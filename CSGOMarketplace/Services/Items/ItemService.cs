using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Models;
using CSGOMarketplace.Services.Items.Models;
using Newtonsoft.Json;

using static CSGOMarketplace.Data.DataConstants;

namespace CSGOMarketplace.Services.Items
{
    public class ItemService : IItemService
    {
        private readonly MarketplaceDbContext data;
        private readonly IMapper mapper;

        public ItemService(MarketplaceDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public ItemQueryServiceModel All(
            string searchTerm = null,
            ItemSorting sorting = ItemSorting.Newest,
            int currentPage = 1,
            int itemsPerPage = int.MaxValue,
            bool publicOnly = true)
        {
            var itemsQuery = this.data.Items.Where(x => !publicOnly || !x.IsSoldOrPendingSale).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                itemsQuery = itemsQuery.Where(item =>
                    (item.Name).ToLower().Contains(searchTerm.ToLower()));
            }

            itemsQuery = sorting switch
            {
                ItemSorting.FloatAscending => itemsQuery.OrderBy(item => item.Float),
                ItemSorting.FloatDescending => itemsQuery.OrderByDescending(item => item.Float),
                ItemSorting.PriceAscending => itemsQuery.OrderBy(item => item.Price),
                ItemSorting.PriceDescending => itemsQuery.OrderByDescending(item => item.Price),
                ItemSorting.Newest or _ => itemsQuery.OrderByDescending(item => item.Id)
            };

            var totalItems = itemsQuery.Count();

            var items = itemsQuery
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(item => new ItemServiceModel()
                {
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    Price = item.Price,
                    Name = item.Name,
                    Float = item.Float ?? 0,
                    ConditionName = item.Condition.Name,
                    InspectUrl = item.InspectUrl,
                    OwnerId = item.UserId
                })
                .ToList();

            return new ItemQueryServiceModel()
            {
                TotalItems = totalItems,
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                Items = items
            };
        }

        public IEnumerable<LatestItemServiceModel> Latest()
            => this.data
                .Items
                .OrderByDescending(i => i.Id)
                .ProjectTo<LatestItemServiceModel>(this.mapper.ConfigurationProvider)
                .Take(3)
                .ToList();
            

        public int Sell(string name, decimal price, double floatValue, string imageUrl, string inspectUrl, string userId,
            string conditionName)
        {
            int? conditionId = conditionName == null
                ? null
                : this.data.Conditions.FirstOrDefault(x => x.Name == conditionName).Id;

            var itemData = new Item()
            {
                Name = name,
                Price = price,
                Float = floatValue,
                ImageUrl = imageUrl,
                InspectUrl = inspectUrl,
                UserId = userId,
                ConditionId = conditionId
            };


            this.data.Items.Add(itemData);
            this.data.SaveChanges();

            return itemData.Id;
        }

        public async Task<ItemServiceModel> CSGOFloatItemInfo(string steamId, string assetId, string d)
        {
            var csgoFloatRequest = DataConstants.CSGOFloatApiEndpoint + $"?s={steamId}&a={assetId}&d={d}";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(csgoFloatRequest);
            string json = null;
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
            var itemInfo = JsonConvert.DeserializeObject<ItemInfoJsonResponseModel>(json);
            var jsonItem = itemInfo.ItemInfo;
            jsonItem.Name = jsonItem.Name.Split('(')[0].Trim();
            var item = this.mapper.Map<ItemServiceModel>(jsonItem);
            item.InspectUrl = DataConstants.SteamItemInspectUrl + $"S{steamId}A{assetId}D{d}";
            item.Price = DataConstants.SamplePrice;
            return item;
        }

        public async Task<string> GetItemImageUrl(string marketHashName)
        {
            var imageResponse = DataConstants.GetImageSteamApi + marketHashName;
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(imageResponse);
            string json = null;
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
            var itemInfo = JsonConvert.DeserializeObject(json);
            var asString = itemInfo.ToString();
            return asString;
        }

        public IEnumerable<ItemServiceModel> ByUser(string userId)
            => GetItems(this.data
                .Items
                .Where(x => x.UserId == userId && !x.IsSoldOrPendingSale));

        public ItemServiceModel ItemById(int id)
            => GetItems(this.data
                .Items
                .Where(x => x.Id == id)).FirstOrDefault();

        private static IEnumerable<ItemServiceModel> GetItems(IQueryable<Item> itemQuery)
            => itemQuery
                .Select(item => new ItemServiceModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Float = item.Float ?? 0,
                    ConditionName = item.Condition.Name,
                    ImageUrl = item.ImageUrl,
                    InspectUrl = item.InspectUrl,
                    Price = item.Price,
                    OwnerId = item.UserId
                })
                .ToList();

        public bool IsByUser(int itemId, string userId)
            => this.data
                .Items
                .Any(item => item.Id == itemId && item.UserId == userId);

        public bool Edit(int id, decimal price)
        {
            var itemData = this.data.Items.Find(id);

            if (itemData == null)
            {
                return false;
            }

            itemData.Price = price;
            this.data.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var itemData = this.data.Items.Find(id);

            if (itemData == null)
            {
                return false;
            }
            data.Items.Remove(itemData);
            data.SaveChanges();
            return true;
        }

        public bool Buy(int itemId, string buyerId)
        {
            var item = this.data.Items.Find(itemId);
            var buyer = this.data.Users.Find(buyerId);

            var sale = new Sale()
            {
                ItemId = item.Id,
            };
            sale.UsersInvolved.Add(this.data.Users.Find(item.UserId));
            sale.UsersInvolved.Add(buyer);
            item.IsSoldOrPendingSale = true;
            data.Sales.Add(sale);
            data.SaveChanges();
            return true;
        }
    }
}
