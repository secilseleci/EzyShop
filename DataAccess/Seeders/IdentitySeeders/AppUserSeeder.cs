using Microsoft.AspNetCore.Identity;
using Models.Identity;
 

namespace DataAccess.Seeders.IdentitySeeders
{
    public static class AppUserSeeder
    {
        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            var defaultAdminEmail = "admin@ezyshop.com";
            var defaultAdminPassword = "Admin.123";

             var adminUser = await userManager.FindByEmailAsync(defaultAdminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = defaultAdminEmail,
                    Email = defaultAdminEmail,
                    Name = "Admin User",
                    EmailConfirmed = true,
                    IsSeller = false,  
                    StoreName = null, 
                    TaxNumber = null, 
                    ContactNumber = "000-000-0000",  
                    Address = "EzyShop Yönetim Merkezi"  
                };

                var result = await userManager.CreateAsync(adminUser, defaultAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
