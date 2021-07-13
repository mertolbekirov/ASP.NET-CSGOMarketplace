using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace CSGOMarketplace.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<MarketplaceDbContext>();

            data.Database.Migrate();

            SeedConditions(data);

            return app;
        }

        private static void SeedConditions(MarketplaceDbContext data)
        {
            if (data.Conditions.Any())
            {
                return;
            }

            data.Conditions.AddRange(new[]
            {
                new Condition { Name = "Factory New" },
                new Condition { Name = "Minimal Wear" },
                new Condition { Name = "Field-Tested" },
                new Condition { Name = "Well-Worn" },
                new Condition { Name = "Battle-Scarred" },
            });

            data.SaveChanges();
        }
    }
}
