using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSGOMarketplace.Data;

namespace CSGOMarketplace.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly MarketplaceDbContext data;

        public StatisticsService(MarketplaceDbContext data)
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalItems = this.data
                .Items
                .Count(i => !i.IsSold);

            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalItems = totalItems,
                TotalUsers = totalUsers
            };
        }
    }
}
