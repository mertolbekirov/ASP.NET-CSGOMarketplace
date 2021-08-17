using System.Linq;
using MyTested.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using CSGOMarketplace.Controllers;
using CSGOMarketplace.Models.Items;
using CSGOMarketplace.Data.Models;
using System.Collections.Generic;
using CSGOMarketplace.Services.Items.Models;
using static CSGOMarketplace.Tests.Data.Items;
using static CSGOMarketplace.Tests.Data.Conditions;

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
                    .WithData(TenNonSoldItems))
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

        [Fact]
        public void GetEditShouldReturnCorrectViewWhenGivenCorrectItemAndUser()
            => MyController<ItemsController>
                .Instance(controller => controller
                    .WithUser()
                    .WithData(TenNonSoldItems))
            .Calling(x => x.Edit(1))
            .ShouldHave()
            .ActionAttributes(attr => attr
                .RestrictingForAuthorizedRequests())
            .Data(data => 
                data.WithSet<Item>(items => 
                    items.FirstOrDefault(item => item.UserId == TestUser.Identifier).Should().NotBeNull()))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ItemFormModel>());

        [Fact]
        public void PostEditShouldEditPriceAndRedirectCorrectly()
            => MyController<ItemsController>
                .Instance(controller => controller
                    .WithUser()
                    .WithData(ItemWithPrice12))
                .Calling(x => x.Edit(1, ValidItemFormModelWithPrice123))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .Data(data =>
                    data.WithSet<Item>(items =>
                        items.FirstOrDefault(item => item.Price == 123).Should().NotBeNull()))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ItemsController>(c => c.All(null)));

        [Fact]
        public void PostSellShouldSellItemCorrectly()
             => MyController<ItemsController>
                .Instance(controller => controller
                    .WithUser()
                    .WithData(ConditionBatteScarred)
                )
                .Calling(x => x.Sell(ValidItemFormModelWithPrice123))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .Data(data =>
                    data.WithSet<Item>(items =>
                        items.FirstOrDefault().Should().NotBeNull()))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ItemsController>(c => c.All(null)));

        [Fact]
        public void GetSellShouldReturnCorrectView()
            => MyController<ItemsController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(x => x.Sell(CorrectSellItemQueryModel))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ItemFormModel>());

        [Fact]
        public void DeleteShouldDeleteCorrectItemWithCorrectOwner()
            => MyController<ItemsController>
                .Instance(controller => controller
                    .WithUser()
                    .WithData(ItemWithPrice12))
                .Calling(x => x.Delete(1))
                .ShouldHave()
                .ActionAttributes(attr => attr
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ItemsController>(c => c.All(null)));
    }
}
