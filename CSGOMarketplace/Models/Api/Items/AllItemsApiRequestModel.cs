namespace CSGOMarketplace.Models.Api.Items
{
    public class AllItemsApiRequestModel
    {

        public string SearchTerm { get; init; }

        public ItemSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int ItemsPerPage { get; init; } = 10;
    }
}
