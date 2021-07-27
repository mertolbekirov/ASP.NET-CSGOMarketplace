using CSGOMarketplace.Services.Items.Models;

namespace CSGOMarketplace.Services.Items
{
    public interface IItemService
    {
        ItemQueryServiceModel All(
            string searchTerm,
            ItemSorting sorting,
            int currentPage,
            int itemsPerPage);

        int Sell(
            string name,
            decimal price,
            double floatValue,
            string imageUrl,
            string inspectUrl,
            string userId,
            string conditionName);
    }
}
