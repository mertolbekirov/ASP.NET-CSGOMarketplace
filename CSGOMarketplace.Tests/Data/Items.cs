using System.Collections.Generic;
using System.Linq;
using CSGOMarketplace.Data.Models;
using MyTested.AspNetCore.Mvc;

namespace CSGOMarketplace.Tests.Data
{
    class Items
    {
        public static IEnumerable<Item> TenPublicItems
            => Enumerable.Range(0, 10).Select(i => new Item
            {
                UserId = TestUser.Identifier
            });
    }
}
