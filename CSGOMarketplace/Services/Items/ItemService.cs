using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Models;
using CSGOMarketplace.Services.Items.Models;
using Newtonsoft.Json;

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

        public ItemQueryServiceModel All(string searchTerm, ItemSorting sorting, int currentPage, int itemsPerPage)
        {
            var itemsQuery = this.data.Items.Where(x => !x.IsSold).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                itemsQuery = itemsQuery.Where(item =>
                    (item.Name).ToLower().Contains(searchTerm.ToLower()));
            }

            itemsQuery = sorting switch
            {
                ItemSorting.Float => itemsQuery.OrderByDescending(item => item.Float),
                ItemSorting.Price or _ => itemsQuery.OrderByDescending(item => item.Price)
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
                    InspectUrl = item.InspectUrl
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

        public IEnumerable<ItemServiceModel> ByUser(string userId)
            => GetItems(this.data
                .Items
                .Where(c => c.UserId == userId));

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

    }
}
