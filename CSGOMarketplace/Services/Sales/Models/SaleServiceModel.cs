using System.Collections.Generic;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Services.Items.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSGOMarketplace.Services.Sales.Models
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
