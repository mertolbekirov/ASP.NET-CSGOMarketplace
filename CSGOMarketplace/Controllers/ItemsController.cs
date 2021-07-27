using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Infrastructure;
using CSGOMarketplace.Models.Items;
using CSGOMarketplace.Services.Items;
using CSGOMarketplace.Services.Items.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSGOMarketplace.Controllers
{
    public class ItemsController : Controller
    {
        private readonly MarketplaceDbContext data;
        private readonly UserManager<User> userManager;
        private readonly IItemService items;
        public ItemsController(MarketplaceDbContext data, IItemService items)
        {
            this.data = data;
            this.items = items;
        }

        public IActionResult All([FromQuery] AllItemsQueryModel query)
        {
            var queryResult = this.items.All(
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllItemsQueryModel.ItemsPerPage);

            query.TotalItems = queryResult.TotalItems;
            query.Items = queryResult.Items;

            return View(query);
        }
        public async Task<IActionResult> ChooseItem()
        {
            var providerKey = await this.GetProviderKey();
            if (providerKey == null)
            {
                return BadRequest();
            }

            return View(new ChooseItemViewModel()
            {
                ProviderKey = providerKey
            });
        }

        [Authorize]
        public async Task<IActionResult> Sell([FromQuery]SellItemQueryModel query)
        {
            var item = await GetItemInfoAsync(await CSGOFloatRequestAsync(query.S, query.A, query.D));
            var inspectUrl = DataConstants.SteamItemInspectUrl + $"S{query.S}A{query.A}D{query.D}";
            return View(new ItemFormModel()
            {
                Name = item.Name,
                Price = DataConstants.SamplePrice,
                Float = item.FloatValue,
                ImageUrl = query.IconUrl,
                InspectUrl = inspectUrl,
                Condition = item.Condition
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sell(ItemFormModel item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            var csgoFloatResponse = await CSGOFloatRequestAsync(item.InspectUrl);
            var csgoFloatItem = await GetItemInfoAsync(csgoFloatResponse);

            if (csgoFloatItem == null)
            {
                return BadRequest();
            }

            this.items.Sell(
                csgoFloatItem.Name,
                item.Price,
                csgoFloatItem.FloatValue,
                csgoFloatItem.ImageUrl,
                item.InspectUrl,
                this.User.GetId(),
                csgoFloatItem.Condition);

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
                    return login.ProviderKey.Split('/').LastOrDefault(); ;
                }
            }
            return null;
        }

        private async Task<HttpResponseMessage> CSGOFloatRequestAsync(string steamId, string assetId, string d)
        {
            var csgoFloatRequest = DataConstants.CSGOFloatApiEndpoint + $"?s={steamId}&a={assetId}&d={d}";
            HttpClient client = new HttpClient();
            return await client.GetAsync(csgoFloatRequest);
        }

        private async Task<HttpResponseMessage> CSGOFloatRequestAsync(string inspectLink)
        {
            var csgoFloatRequest = DataConstants.CSGOFloatApiEndpoint + inspectLink;
            HttpClient client = new HttpClient();
            return await client.GetAsync(csgoFloatRequest);
        }
        private async Task<ItemJsonResponseModel> GetItemInfoAsync(HttpResponseMessage response)
        {
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
            var item = itemInfo.ItemInfo;
            if (item.Condition == null)
            {
                item.Name = item.FullName;
            }

            return item;
        }
    }
}
