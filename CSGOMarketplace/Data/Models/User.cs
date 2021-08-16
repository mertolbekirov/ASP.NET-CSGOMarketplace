using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CSGOMarketplace.Data.Models
{
    public class User : IdentityUser
    {
        public ICollection<Item> Items { get; init; } = new List<Item>();

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
