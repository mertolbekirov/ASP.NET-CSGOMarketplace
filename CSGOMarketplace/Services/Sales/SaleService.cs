using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using CSGOMarketplace.Services.Items.Models;
using CSGOMarketplace.Services.Sales.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CSGOMarketplace.Services.Sales
{
    public class SaleService : ISaleService
    {
        private readonly MarketplaceDbContext data;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public SaleService(MarketplaceDbContext data, IMapper mapper, UserManager<User> userManager)
        {
            this.data = data;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IEnumerable<SaleServiceModel> Unresolved()
        {
            var unresolved = this.data
                .Sales
                .Include(x => x.UsersInvolved)
                .Include(x => x.Item)
                .ThenInclude(x => x.Condition)
                .Where(x => !x.IsResolved)
                .ToList();

            List<SaleServiceModel> unresolvedModels = new List<SaleServiceModel>();
            foreach (var sale in unresolved)
            {
                unresolvedModels.Add(new SaleServiceModel
                {
                    Id = sale.Id,
                    ItemId = sale.ItemId,
                    Item = mapper.Map<ItemServiceModel>(sale.Item),
                    UsersInvolved = sale.UsersInvolved.Select(GetProviderKeyByUser).ToList()
                });
            }

            return unresolvedModels;
        }

        public IEnumerable<SaleServiceModel> Resolved()
        {
            var resolved = this.data
                .Sales
                .Include(x => x.UsersInvolved)
                .Include(x => x.Item)
                .ThenInclude(x => x.Condition)
                .Where(x => x.IsResolved)
                .ToList();

            List<SaleServiceModel> unresolvedModels = new List<SaleServiceModel>();
            foreach (var sale in resolved)
            {
                unresolvedModels.Add(new SaleServiceModel
                {
                    Id = sale.Id,
                    ItemId = sale.ItemId,
                    Item = mapper.Map<ItemServiceModel>(sale.Item),
                    UsersInvolved = sale.UsersInvolved.Select(GetProviderKeyByUser).ToList()
                });
            }

            return unresolvedModels;
        }


        public bool Resolve(int saleId)
        {
            var sale = this.data.Sales.Find(saleId);
            if (sale == null)
            {
                return false;
            }
            sale.IsResolved = true;
            this.data.SaveChanges();
            return true;
        }

        public bool Delete(int saleId)
        {
            var sale = this.data.Sales.Find(saleId);
            if (sale == null)
            {
                return false;
            }

            var item = this.data.Items.Find(sale.ItemId);
            item.IsSoldOrPendingSale = false;
            this.data.Sales.Remove(sale);
            this.data.SaveChanges();
            return true;
        }


        private string GetProviderKeyByUser(User user)
        {
            var logins = Task.Run(() => userManager.GetLoginsAsync(user)).Result;
            foreach (var login in logins)
            {
                if (login.ProviderDisplayName == "Steam")
                {
                    return login.ProviderKey.Split('/').LastOrDefault(); ;
                }
            }
            return null;
        }
    }
}
