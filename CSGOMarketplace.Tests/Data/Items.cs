using System.Collections.Generic;
using System.Linq;
using MyTested.AspNetCore.Mvc;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Models.Items;

namespace CSGOMarketplace.Tests.Data
{
    class Items
    {
        public static IEnumerable<Item> TenNonSoldItems
            => Enumerable.Range(0, 10).Select(i => new Item
            {
                UserId = TestUser.Identifier
            });

        public static ItemFormModel ValidItemFormModelWithPrice123
            => new ItemFormModel
            {
                Name = "★ M9 Bayonet | Night",
                Float = 0.05,
                ConditionName = "Factory New",
                ImageUrl = "https://cdn.steamcommunity.com/economy/image/-9a81dlWLwJ2UUGcVs_nsVtzdOEdtWwKGZZLQHTxDZ7I56KU0Zwwo4NUX4oFJZEHLbXH5ApeO4YmlhxYQknCRvCo04DEVlxkKgpovbSsLQJf3qr3czxb49KzgL-YmMj6OrzZglRZ7cRnk9bN9J7yjRrsrRdvMjz0cY-QdwE4YF_S-Vm4yOi5hpHo7szOyHswvyUq4C3bmEG2n1gSOUNlScIM",
                InspectUrl = "steam://rungame/730/76561202255233023/+csgo_econ_action_preview%20S76561198181212486A18588979720D1039771199931650402",
                Price = 123,
            };

        public static Item ItemWithPrice12
            => new Item
            {
                UserId = TestUser.Identifier,
                Price = 12
            };

        public static SellItemQueryModel CorrectSellItemQueryModel
            => new SellItemQueryModel
            {
                A = "12033435440",
                D = "5657167653510852271",
                IconUrl = "https://cdn.steamcommunity.com/economy/image/-9a81dlWLwJ2UUGcVs_nsVtzdOEdtWwKGZZLQHTxDZ7I56KU0Zwwo4NUX4oFJZEHLbXH5ApeO4YmlhxYQknCRvCo04DEVlxkKgpou6ryFAR17P7YJnBB49G7lY6PkuXLP7LWnn9u5MRjjeyPp9qljAey-URqZjr7J9CSd1NvNQmD_wDslei605K9tJqcmHAwuCAq7GGdwUJMw04E0g",
                S = "76561198181212486"
            };

    }
}
