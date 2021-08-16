using System.Collections.Generic;
using CSGOMarketplace.Services.Sales.Models;

namespace CSGOMarketplace.Services.Sales
{
    public interface ISaleService
    {
        public IEnumerable<SaleServiceModel> Unresolved();

        public IEnumerable<SaleServiceModel> Resolved();

        public bool Resolve(int saleId);

        public bool Delete(int saleId);
    }
}
