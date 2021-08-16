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

            ViewBag.Unresolved = true;

            return View(salesToResolve);
        }
            
        public IActionResult ResolvedSales()
        {
            var resolvedSales = this.sales.Resolved();

            ViewBag.Resolved = true;

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

        public IActionResult Unresolve(int id)
        {
            var isUnresolved = this.sales.Unresolve(id);
            if (!isUnresolved)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(ResolvedSales));
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
