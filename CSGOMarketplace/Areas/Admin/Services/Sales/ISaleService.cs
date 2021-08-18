using System.Collections.Generic;
using CSGOMarketplace.Areas.Admin.Services.Sales.Models;

namespace CSGOMarketplace.Areas.Admin.Services.Sales
{
    public interface ISaleService
    {
        public IEnumerable<SaleServiceModel> Unresolved();

        public IEnumerable<SaleServiceModel> Resolved();

        public bool Resolve(int saleId);
        public bool Unresolve(int saleId);

        public bool Delete(int saleId);
    }
}
