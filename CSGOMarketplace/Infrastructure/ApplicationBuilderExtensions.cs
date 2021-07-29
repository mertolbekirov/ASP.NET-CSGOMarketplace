using CSGOMarketplace.Areas.Admin;
using CSGOMarketplace.Data;
using CSGOMarketplace.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CSGOMarketplace.Areas.Admin.AdminConstants;

namespace CSGOMarketplace.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedConditions(services);
            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<MarketplaceDbContext>();

            data.Database.Migrate();
        }

        private static void SeedConditions(IServiceProvider services)
        {
            var data = services.GetRequiredService<MarketplaceDbContext>();

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

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "mertol2002@gmail.com";
                    const string adminPassword = "pw123123pw";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
