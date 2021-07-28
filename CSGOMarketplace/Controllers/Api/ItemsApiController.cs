using CSGOMarketplace.Models.Api.Items;
using CSGOMarketplace.Services.Items;
using CSGOMarketplace.Services.Items.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSGOMarketplace.Controllers.Api
{
    [ApiController]
    [Route("api/cars")]
    public class ItemsApiController : ControllerBase
    {
        private readonly IItemService Items;

        public ItemsApiController(IItemService Items)
            => this.Items = Items;

        [HttpGet]
        public ItemQueryServiceModel All([FromQuery] AllItemsApiRequestModel query)
            => this.Items.All(
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.ItemsPerPage);
    }
}
