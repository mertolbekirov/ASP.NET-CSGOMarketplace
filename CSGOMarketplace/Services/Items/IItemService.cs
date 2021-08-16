using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CSGOMarketplace.Models;
using CSGOMarketplace.Services.Items.Models;

namespace CSGOMarketplace.Services.Items
{
    public interface IItemService
    {
        ItemQueryServiceModel All(
            string searchTerm = null,
            ItemSorting sorting = ItemSorting.Price,
            int currentPage = 1,
            int itemsPerPage = int.MaxValue,
            bool publicOnly = true);

        int Sell(
            string name,
            decimal price,
            double floatValue,
            string imageUrl,
            string inspectUrl,
            string userId,
            string conditionName);

        Task<ItemServiceModel> CSGOFloatItemInfo(string steamId, string assetId, string d);

        IEnumerable<ItemServiceModel> ByUser(string userId);

        ItemServiceModel ItemById(int id);

        bool IsByUser(int itemId, string userId);

        bool Edit(int id, decimal price);

        bool Delete(int id);

        public IEnumerable<LatestItemServiceModel> Latest();

        public bool Buy(int itemId, string buyerId);
    }
}
