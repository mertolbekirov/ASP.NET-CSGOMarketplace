using System.Linq;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Services.Items.Models;

namespace CSGOMarketplace.Services.Items
{
    public class ItemService : IItemService
    {
        private readonly MarketplaceDbContext data;

        public ItemService(MarketplaceDbContext data)
        {
            this.data = data;
        }

        public ItemQueryServiceModel All(string searchTerm, ItemSorting sorting, int currentPage, int itemsPerPage)
        {
            var itemsQuery = this.data.Items.Where(x => !x.IsSold).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                itemsQuery = itemsQuery.Where(item =>
                    (item.Name).ToLower().Contains(searchTerm.ToLower()));
            }

            itemsQuery = sorting switch
            {
                ItemSorting.Float => itemsQuery.OrderByDescending(item => item.Float),
                ItemSorting.Price or _ => itemsQuery.OrderByDescending(item => item.Price)
            };

            var totalItems = itemsQuery.Count();

            var items = itemsQuery
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(item => new ItemServiceModel()
                {
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    Price = item.Price,
                    Name = item.Name,
                    Float = item.Float,
                    Condition = item.Condition.Name,
                    InspectUrl = item.InspectUrl
                })
                .ToList();

            return new ItemQueryServiceModel()
            {
                TotalItems = totalItems,
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                Items = items
            };
        }

        public int Sell(string name, decimal price, double floatValue, string imageUrl, string inspectUrl, string userId,
            string conditionName)
        {
            int? conditionId = conditionName == null
                ? null
                : this.data.Conditions.FirstOrDefault(x => x.Name == conditionName).Id;

            var itemData = new Item()
            {
                Name = name,
                Price = price,
                Float = floatValue,
                ImageUrl = imageUrl,
                InspectUrl = inspectUrl,
                UserId = userId,
                ConditionId = conditionId
            };


            this.data.Items.Add(itemData);
            this.data.SaveChanges();

            return itemData.Id;
        }
    }
}
