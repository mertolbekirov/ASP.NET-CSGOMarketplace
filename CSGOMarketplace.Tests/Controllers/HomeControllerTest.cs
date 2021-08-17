using System;
using System.Collections.Generic;
using CSGOMarketplace.Controllers;
using CSGOMarketplace.Services.Items.Models;
using FluentAssertions;
using MyTested.AspNetCore.Mvc;
using Xunit;

using static CSGOMarketplace.WebConstants.Cache;
using static CSGOMarketplace.Tests.Data.Items;

namespace CSGOMarketplace.Tests.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectViewWithModel()
            => MyController<HomeController>
                .Instance(controller => controller
                    .WithData(TenPublicCars))
                .Calling(c => c.Index())
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(LatestItemsCacheKey)
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))
                        .WithValueOfType<List<LatestItemServiceModel>>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<LatestItemServiceModel>>()
                    .Passing(model => model.Should().HaveCount(3)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
    }
}
