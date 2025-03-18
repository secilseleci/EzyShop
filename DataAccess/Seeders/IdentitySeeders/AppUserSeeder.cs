using Microsoft.AspNetCore.Identity;
using Models.Identity;

namespace DataAccess.Seeders.IdentitySeeders
{
    public static class AppUserSeeder
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            // ✅ 1. Admin
            await CreateUserAsync(userManager,
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "admin@ezyshop.com",
                "Admin User",
                "Admin.123",
                "Admin",
                false);

            // ✅ 2. Müşteriler (Müşteri olarak ekleniyorlar, Seller eklenmiyor!)
            await CreateUserAsync(userManager,
                Guid.Parse("44444444-4444-4444-4444-444444444444"),
                "derya.karaman.ezyshop@gmail.com",
                "Derya Karaman",
                "Customer.123",
                "Customer",
                false);

            await CreateUserAsync(userManager,
                Guid.Parse("55555555-5555-5555-5555-555555555555"),
                "nur.biral.ezyshop@gmail.com",
                "Nur Biral",
                "Customer.123",
                "Customer",
                false);

            Console.WriteLine("✅ Müşteriler ve Admin eklendi.");
        }

        private static async Task CreateUserAsync(
            UserManager<AppUser> userManager,
            Guid userId,
            string email,
            string name,
            string? password,
            string role,
            bool isSeller = false)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser
                {
                    Id = userId,
                    UserName = email,
                    Email = email,
                    Name = name,
                    EmailConfirmed = true,
                    IsSeller = isSeller,
                    IsActive = true,
                    PhoneNumber = "0000000000",
                    Address = "Not Provided"
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
