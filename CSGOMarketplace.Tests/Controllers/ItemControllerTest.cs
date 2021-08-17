using System.Linq;
using MyTested.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using CSGOMarketplace.Controllers;
using CSGOMarketplace.Models.Items;
using CSGOMarketplace.Data.Models;
using static CSGOMarketplace.Tests.Data.Items;
using System.Collections.Generic;
using CSGOMarketplace.Services.Items.Models;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;

namespace CSGOMarketplace.Tests.Controllers
{
    public class ItemControllerTest
    {
        [Fact]
        public void AllShouldReturnView()
            => MyController<ItemsController>
            .Instance()
            .Calling(x => x.All(new AllItemsQueryModel { }))
            .ShouldReturn()
            .View();

        [Fact]
        public void MineShouldReturnCorrectCarsAndReturnView()
            => MyController<ItemsController>
                .Instance(controller => controller
                    .WithUser()
                    .WithData(TenPublicItems))
            .Calling(x => x.Mine())
            .ShouldHave()
            .ActionAttributes(attr => attr
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<IEnumerable<ItemServiceModel>>()
                .Passing(items =>
                    items.Count().Should().Be(10)));
    }
}
