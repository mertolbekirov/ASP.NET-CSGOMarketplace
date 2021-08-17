using System.Linq;
using MyTested.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using CSGOMarketplace.Areas.Admin.Controllers;
using CSGOMarketplace.Data.Models;
using static CSGOMarketplace.Areas.Admin.AdminConstants;
using static CSGOMarketplace.Tests.Data.Sales;

namespace CSGOMarketplace.Tests.Controllers.Admin
{
    public class SalesControllerTest
    {
        [Fact]
        public void UnresolvedSalesShouldReturnViewCorrectly()
            => MyController<SalesController>
                .Instance(controller => controller
                    .WithData(TenUnresolvedSales)
                    .WithUser(user => user.InRole(AdministratorRoleName)
                    ))
                .Calling(x => x.UnresolvedSales())
                .ShouldReturn()
                .View();

        [Fact]
        public void ResolvedSalesShouldReturnViewCorrectly()
            => MyController<SalesController>
                .Instance(controller => controller
                    .WithData(TenResolvedSales)
                    .WithUser(user => user.InRole(AdministratorRoleName)
                    ))
                .Calling(x => x.ResolvedSales())
                .ShouldReturn()
                .View();

        [Fact]
        public void ResolveShouldResolveTheSaleAndSaveToDbCorrectly()
            => MyController<SalesController>
                .Instance(controller => controller
                    .WithData(UnresolvedSale)
                    .WithUser(user => user.InRole(AdministratorRoleName)
                    ))
                .Calling(x => x.Resolve(1))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Sale>(sales => sales
                        .FirstOrDefault(sale => sale.IsResolved == true).Should().NotBeNull()))
                .AndAlso()
                .ShouldReturn()
                .Redirect();

        [Fact]
        public void UnResolveShouldResolveTheSaleAndSaveToDbCorrectly()
            => MyController<SalesController>
                .Instance(controller => controller
                    .WithData(ResolvedSale)
                    .WithUser(user => user.InRole(AdministratorRoleName)
                    ))
                .Calling(x => x.Unresolve(1))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Sale>(sales => sales
                        .FirstOrDefault(sale => sale.IsResolved == false).Should().NotBeNull()))
                .AndAlso()
                .ShouldReturn()
                .Redirect();
    }
}
