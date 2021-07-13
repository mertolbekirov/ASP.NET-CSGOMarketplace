using CSGOMarketplace.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSGOMarketplace.Data
{
    public class MarketplaceDbContext : IdentityDbContext
    {
        public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options)
            : base(options)
        {
        }


        public DbSet<Sale> Sales { get; init; }

        public DbSet<Condition> Conditions { get; init; }

    }
}
