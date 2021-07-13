using System.Collections.Generic;
using System.Linq;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Models.Sales;
using Microsoft.AspNetCore.Mvc;


namespace CSGOMarketplace.Controllers
{
    public class SalesController : Controller
    {

        private readonly MarketplaceDbContext data;

        public SalesController(MarketplaceDbContext data)
            => this.data = data;

        public IActionResult Sell() => View(new AddSaleViewModel()
        {
            Conditions = this.GetSaleConditions()
        });

        private IEnumerable<SaleConditionViewModel> GetSaleConditions()
            => this.data
                .Conditions
                .Select(c => new SaleConditionViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
    }

    
}
