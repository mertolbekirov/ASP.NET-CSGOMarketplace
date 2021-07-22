using CSGOMarketplace.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSGOMarketplace.Data
{
    public class MarketplaceDbContext : IdentityDbContext<ApplicationUser>
    {
        public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options)
            : base(options)
        {
        }


        public DbSet<Item> Items { get; init; }

        public DbSet<Condition> Conditions { get; init; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Item>()
            .Property(b => b.Price)
            .HasPrecision(15, 2);

            builder
                .Entity<Item>()
                .HasOne(c => c.Condition)
                .WithMany(c => c.Items)
                .HasForeignKey(c => c.ConditionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Item>()
                .HasOne(i => i.Owner)
                .WithMany(u => u.Items)
                .OnDelete(DeleteBehavior.Restrict);
            
            base.OnModelCreating(builder);
        }
    }
}
