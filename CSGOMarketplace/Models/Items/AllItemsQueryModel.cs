using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSGOMarketplace.Models.Items
{
    public class AllItemsQueryModel
    {
        public const int ItemsPerPage = 3;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public ItemSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalItems { get; set; }

        public IEnumerable<ItemListingViewModel> Items { get; set; }
    }
}
