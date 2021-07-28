using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOMarketplace.Services.Statistics
{
    public class StatisticsServiceModel
    {
        public int TotalItems { get; init; }

        public int TotalUsers { get; init; }

        public int TotalSales { get; init; }
    }
}
