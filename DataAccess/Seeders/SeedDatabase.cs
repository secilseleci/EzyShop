using DataAccess.Seeders.EntitySeeders;
using DataAccess.Seeders.IdentitySeeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models.Identity;

namespace DataAccess.SeedDatabase
{
    public static class SeedDatabase
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Identity Seeders
            await RoleSeeder.SeedRolesAsync(roleManager);
            await AppUserSeeder.SeedAdminUserAsync(userManager);

            // Entity Seeders
            await CategorySeeder.SeedCategoriesAsync(dbContext);
        }
    }
}
