using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CSGOMarketplace.Data.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<Item> Items { get; init; } = new List<Item>();

        public IEnumerable<Sale> Sales { get; set; } = new List<Sale>();
    }
}
