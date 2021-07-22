using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOMarketplace.Models.Items
{
    public class ItemListingViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public double Float { get; init; }

        public string Condition { get; init; }

        public string ImageUrl { get; init; }

        public decimal Price { get; init; }
    }
}
