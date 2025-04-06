using Microsoft.AspNetCore.Identity;
using Models.Identity;

namespace DataAccess.Seeders.IdentitySeeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            var roles = new[] { "Admin", "Seller", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }
        }
    }
}
