using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using Microsoft.AspNetCore.Identity;
using CSGOMarketplace.Infrastructure;
using CSGOMarketplace.Models.Items;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CSGOMarketplace.Controllers
{
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

        [Authorize]
        public IActionResult Sell() => View(new AddItemForModel()
        {
            Conditions = this.GetSaleConditions()
        });

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sell(AddItemForModel item)
        {
            if (!this.data.Conditions.Any(c => c.Id == item.ConditionId))
            {
                this.ModelState.AddModelError(nameof(item.ConditionId), "Condition does not exist.");
            }

            if (!ModelState.IsValid)
            {
                item.Conditions = this.GetSaleConditions();

                return View(item);
            }

            var saleData = new Item()
            {
                Name = item.Name,
                Price = item.Price,
                Float = item.Float,
                ImageUrl = item.ImageUrl,
                ConditionId = item.ConditionId,
                ApplicationUserId = this.User.GetId()
            };

            
            this.data.Items.Add(saleData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<ItemConditionViewModel> GetSaleConditions()
            => this.data
                .Conditions
                .Select(c => new ItemConditionViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

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
