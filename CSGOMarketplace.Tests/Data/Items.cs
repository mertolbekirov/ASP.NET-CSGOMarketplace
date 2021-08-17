using System.Collections.Generic;
using System.Linq;
using CSGOMarketplace.Data.Models;

namespace CSGOMarketplace.Tests.Data
{
    class Items
    {
        public static IEnumerable<Item> TenPublicCars
            => Enumerable.Range(0, 10).Select(i => new Item
            {
            });
    }
}
