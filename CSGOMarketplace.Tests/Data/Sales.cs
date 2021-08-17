using CSGOMarketplace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOMarketplace.Tests.Data
{
    class Sales
    {
        public static IEnumerable<Sale> TenUnresolvedSales
            => Enumerable.Range(0, 10).Select(i => new Sale
            {
                IsResolved = false
            });

        public static IEnumerable<Sale> TenResolvedSales
            => Enumerable.Range(0, 10).Select(i => new Sale
            {
                IsResolved = true
            });

        public static Sale UnresolvedSale
            => new Sale
            {
                IsResolved = false
            };

        public static Sale ResolvedSale
            => new Sale
            {
                IsResolved = false
            };
    }
}
