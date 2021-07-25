using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using Microsoft.AspNetCore.Identity;
using CSGOMarketplace.Infrastructure;
using CSGOMarketplace.Models.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CSGOMarketplace.Controllers
{
    using static DataConstants;
    public class ItemsController : Controller
    {
        private readonly MarketplaceDbContext data;
        private readonly UserManager<ApplicationUser> userManager;

        public ItemsController(MarketplaceDbContext data, UserManager<ApplicationUser> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public IActionResult All([FromQuery] AllItemsQueryModel query)
        {
            var itemsQuery = this.data.Items.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                itemsQuery = itemsQuery.Where(item =>
                    (item.Name).ToLower().Contains(query.SearchTerm.ToLower()));
            }

            itemsQuery = query.Sorting switch
            {
                ItemSorting.Float => itemsQuery.OrderByDescending(item => item.Float),
                ItemSorting.Price or _ => itemsQuery.OrderByDescending(item => item.Price)
            };

            var totalItems = itemsQuery.Count();

            var items = itemsQuery
                .Skip((query.CurrentPage - 1) * AllItemsQueryModel.ItemsPerPage)
                .Take(AllItemsQueryModel.ItemsPerPage)
                .Select(item => new ItemListingViewModel()
                {
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    Price = item.Price,
                    Name = item.Name,
                    Float = item.Float,
                    Condition = item.Condition.Name,
                })
                .ToList();

            query.TotalItems = totalItems;
            query.Items = items;

            return View(query);
        }
        public async Task<IActionResult> ChooseItem()
        {
            var providerKey = await this.GetProviderKey();
            providerKey = providerKey.Split('/').LastOrDefault();
            if (providerKey == null)
            {
                return View("/Error");
            }

            var model = new ChooseItemViewModel()
            {
                ProviderKey = providerKey
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Sell([FromQuery]SellItemQueryModel query)
        {
            var csgoFloatRequest = DataConstants.CSGOFloatApiEndpoint + $"?s={query.S}&a={query.A}&d={query.D}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(csgoFloatRequest);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var itemInfo = JsonConvert.DeserializeObject<ItemInfoJsonResponseModel>(json);
                var item = itemInfo.ItemInfo;
                var inspectUrl = DataConstants.SteamItemInspectUrl + $"S{query.S}A{query.A}D{query.D}";
                return View(new AddItemFormModel()
                {
                    Name = item.ItemName,
                    Price = DataConstants.SamplePrice,
                    Float = double.Parse(item.FloatValue),
                    ImageUrl = item.ImageUrl,
                    InspectUrl = inspectUrl,
                    Condition = item.WearName
                });
            }
            else
            {
                return View("/Error");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Sell(AddItemFormModel item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            var saleData = new Item()
            {
                Name = item.Name,
                Price = item.Price,
                Float = item.Float,
                ImageUrl = item.ImageUrl,
                ApplicationUserId = this.User.GetId()
            };

            
            this.data.Items.Add(saleData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }


        private async Task<string> GetProviderKey()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var logins = await userManager.GetLoginsAsync(user);
            foreach (var login in logins)
            {
                if (login.ProviderDisplayName == "Steam")
                {
                    return login.ProviderKey;
                }
            }

            return null;
        }
    }

    
}
