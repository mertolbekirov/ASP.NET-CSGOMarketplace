using AutoMapper;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Infrastructure;
using CSGOMarketplace.Models.Items;
using CSGOMarketplace.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CSGOMarketplace.Services.Items.Models;
using static CSGOMarketplace.WebConstants;

namespace CSGOMarketplace.Controllers
{
    public class ItemsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IItemService items;
        private readonly IMapper mapper;

        public ItemsController(IItemService items, UserManager<User> userManager, IMapper mapper)
        {
            this.items = items;
            this.userManager = userManager;
            this.mapper = mapper;
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

        [Authorize]
        public async Task<IActionResult> ChooseItem()
        {
            var providerKey = await this.GetProviderKeyByClaims();
            if (providerKey == null)
            {
                TempData[GlobalMessageKey] = "Sign in with Steam to unlock this feature.";

                return RedirectToAction(nameof(All));
            }

            return View(new ChooseItemViewModel()
            {
                ProviderKey = providerKey
            });
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myItems = this.items.ByUser(this.User.Id());

            return View(myItems);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.Id();

            var item = this.items.ItemById(id);

            if (item == null)
            {
                return BadRequest();
            }

            if (item.OwnerId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var itemForm = this.mapper.Map<ItemFormModel>(item);
            return View(itemForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, ItemFormModel car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            if (!this.items.IsByUser(id, this.User.Id()) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.items.Edit(id, car.Price);

            TempData[GlobalMessageKey] = "The price of your item was edited successfully";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> Sell([FromQuery]SellItemQueryModel query)
        {
            var csgoFloatItemInfo = await this.items.CSGOFloatItemInfo(query.S, query.A, query.D);
            if (csgoFloatItemInfo == null)
            {
                return BadRequest();
            }
            ReplaceCSGOFloatItemImageUrl(csgoFloatItemInfo);
            var item = this.mapper.Map<ItemFormModel>(csgoFloatItemInfo);
            item.ImageUrl = query.IconUrl;
            return View(item);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sell(ItemFormModel item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            var queryParams = item.InspectUrl.Split(new char[] {'S', 'A', 'D'});
            var csgoFloatItemInfo = await this.items.CSGOFloatItemInfo(queryParams[1], queryParams[2], queryParams[3]);
           

            if (csgoFloatItemInfo == null)
            {
                return BadRequest();
            } 
            
            ReplaceCSGOFloatItemImageUrl(csgoFloatItemInfo);

            this.items.Sell(
                csgoFloatItemInfo.Name,
                item.Price,
                csgoFloatItemInfo.Float,
                csgoFloatItemInfo.ImageUrl,
                item.InspectUrl,
                this.User.Id(),
                csgoFloatItemInfo.ConditionName);

            TempData[GlobalMessageKey] = "Your item was put for sale successfully";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.Id();

            var item = this.items.ItemById(id);

            if (item == null)
            {
                return BadRequest();
            }

            if (item.OwnerId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            this.items.Delete(id);

            TempData[GlobalMessageKey] = "Item was deleted successfully";

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Buy(int id)
        {
            var buyerId = this.User.Id();

            var item = this.items.ItemById(id);

            if (await GetProviderKeyByClaims() == null)
            {
                TempData[GlobalMessageKey] = "Sign in with Steam to unlock this feature.";

                return RedirectToAction(nameof(All));
            }

            if (item == null || buyerId == item.OwnerId)
            {
                return BadRequest();
            }

            this.items.Buy(id, buyerId);

            TempData[GlobalMessageKey] = "You successfully requested to buy the item. Be ready to be contacted by and admin via steam to complete the exchange.";

            return RedirectToAction(nameof(All));
        }

        private async Task<string> GetProviderKeyByClaims()
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

        private void ReplaceCSGOFloatItemImageUrl(ItemServiceModel csgoFloatItem)
        {
            if (csgoFloatItem.ConditionName == null)
            {
                csgoFloatItem.ImageUrl = DataConstants.GetImageSteamApi + csgoFloatItem.Name;
            }
            else
            {
                csgoFloatItem.ImageUrl = DataConstants.GetImageSteamApi + csgoFloatItem.Name + " (" +
                                         csgoFloatItem.ConditionName + ")";

            }
        }

    }
}
