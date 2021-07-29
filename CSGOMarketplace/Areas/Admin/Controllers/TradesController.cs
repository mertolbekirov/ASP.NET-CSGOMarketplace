using Microsoft.AspNetCore.Mvc;

namespace CSGOMarketplace.Areas.Admin.Controllers
{
    public class TradesController: AdminController
    {
        public IActionResult Index() => View();
    }
}
