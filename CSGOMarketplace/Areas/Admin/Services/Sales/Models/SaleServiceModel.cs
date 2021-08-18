using System.Collections.Generic;
using CSGOMarketplace.Services.Items.Models;

namespace CSGOMarketplace.Areas.Admin.Services.Sales.Models
{
    public class SaleServiceModel
    {
        public int Id { get; init; }

        public int ItemId { get; init; }

        public ItemServiceModel Item { get; init; }

        public decimal GetPrice()
            => this.Item.Price;

        public IList<string> UsersInvolved { get; set; } = new List<string>();
    }
}
