using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CSGOMarketplace.Data.Models
{
    public class Sale
    {
        public int Id { get; init; }

        public int ItemId { get; set; }

        public Item Item { get; set; }

        public IList<User> UsersInvolved { get; set; } = new List<User>();

        public bool IsResolved { get; set; }
    }
}
