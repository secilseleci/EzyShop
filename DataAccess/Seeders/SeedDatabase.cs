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

            // ✅ 1. Roller 
            await RoleSeeder.SeedRolesAsync(roleManager);

            // ✅ 2. Kullanıcılar 
            await AppUserSeeder.SeedUsersAsync(userManager);

            // ✅ 4. Diğer Entityler
            await CategorySeeder.SeedCategoriesAsync(dbContext);
            await SellerApplicationSeeder.SeedSellerApplicationsAsync(dbContext);
            await ShopSeeder.SeedShopsAsync(dbContext);
            await ProductSeeder.SeedProductsAsync(dbContext);
            await OrderSeeder.SeedOrdersAsync(dbContext);




        }
    }
}
