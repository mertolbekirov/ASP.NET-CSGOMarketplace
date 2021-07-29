using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CSGOMarketplace.Areas.Admin.AdminConstants;

namespace CSGOMarketplace.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public abstract class AdminController : Controller
    {
    }
}
