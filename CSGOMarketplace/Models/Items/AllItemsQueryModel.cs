using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSGOMarketplace.Services.Items.Models;

namespace CSGOMarketplace.Models.Items
{
    public class AllItemsQueryModel
    {
        public const int ItemsPerPage = 6;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public ItemSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalItems { get; set; }

        public IEnumerable<ItemServiceModel> Items { get; set; }
    }
}
