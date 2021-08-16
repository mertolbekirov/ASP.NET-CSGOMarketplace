using CSGOMarketplace.Controllers;
using CSGOMarketplace.Services.Items;
using CSGOMarketplace.Services.Sales;
using Microsoft.AspNetCore.Mvc;

namespace CSGOMarketplace.Areas.Admin.Controllers
{
    public class SalesController: AdminController
    {
        private readonly ISaleService sales;

        public SalesController(ISaleService sales) => this.sales = sales;

        public IActionResult UnresolvedSales()
        {
            var salesToResolve = this.sales.Unresolved();

            return View(salesToResolve);
        }

        public IActionResult ResolvedSales()
        {
            var resolvedSales = this.sales.Resolved();

            return View(resolvedSales);
        }

        public IActionResult Resolve(int id)
        {
            var isResolved = this.sales.Resolve(id);
            if (!isResolved)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(UnresolvedSales));
        }

        public IActionResult Delete(int id)
        {
            var isDeleted = this.sales.Delete(id);
            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(UnresolvedSales));
        }
    }
}
