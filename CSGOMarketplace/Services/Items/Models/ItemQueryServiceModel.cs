using System.Collections.Generic;

namespace CSGOMarketplace.Services.Items.Models
{
    public class ItemQueryServiceModel
    {
        public int CurrentPage { get; init; }
        public int ItemsPerPage { get; init; }
        public int TotalItems { get; init; }

        public IEnumerable<ItemServiceModel> Items { get; init; }
    }
}
