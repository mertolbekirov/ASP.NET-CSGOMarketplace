using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOMarketplace.Services.Items.Models
{
    public class LatestItemServiceModel
    {
        public string Name { get; init; }

        public string Condition { get; init; }

        public double? Float { get; init; }

        public string ImageUrl { get; set; }
    }
}
