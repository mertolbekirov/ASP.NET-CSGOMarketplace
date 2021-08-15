using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOMarketplace.Data.Models
{
    public class UserSale
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int SaleId { get; set; }

        public Sale Sale { get; set; }
    }
}
