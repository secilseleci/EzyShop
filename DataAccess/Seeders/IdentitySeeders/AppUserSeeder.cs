using Microsoft.AspNetCore.Identity;
using Models.Identity;

namespace DataAccess.Seeders.IdentitySeeders
{
    public static class AppUserSeeder
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            // ✅ 1. Admin
            await CreateUserAsync(userManager, Guid.Parse("11111111-1111-1111-1111-111111111111"), "admin@ezyshop.com", "Admin User", "Admin.123", "Admin", false);

            // ✅ 2. Seller'lar  
            await CreateUserAsync(userManager, Guid.Parse("22222222-2222-2222-2222-222222222222"), "secil.seleci@gmail.com", "Seçil Seleci", "Customer.123", "Seller", true, true, "FirstShop", "1234567890", "555-111-2222", "Istanbul");
            await CreateUserAsync(userManager, Guid.Parse("33333333-3333-3333-3333-333333333333"), "bernahakmanezyshop@gmail.com", "Berna Hakman", "Customer.123", "Seller", true, true, "BernaStore", "9876543210", "555-333-4444", "Ankara");

            // ✅ 3. Customer
            await CreateUserAsync(userManager, Guid.Parse("44444444-4444-4444-4444-444444444444"), "deryaafacanezyshop@gmail.com", "Derya Afacan", "Customer.123", "Customer");
            await CreateUserAsync(userManager, Guid.Parse("55555555-5555-5555-5555-555555555555"), "nursurerezyshop@gmail.com", "Nur Sürer", "Customer.123", "Customer");

            Console.WriteLine("✅ Kullanıcılar başarıyla eklendi!");
        }

        private static async Task CreateUserAsync(
            UserManager<AppUser> userManager,
            Guid userId, string email, string name, string? password, string role,
            bool isSeller = false, bool isActive = true,
            string? storeName = null, string? taxNumber = null,
            string? contactNumber = null, string? address = null)
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
                    IsActive = isActive, // 🔥 Satıcı onaylı mı değil mi artık belli!
                    StoreName = storeName,
                    TaxNumber = taxNumber,
                    ContactNumber = contactNumber,
                    Address = address
                };

                if (!string.IsNullOrEmpty(password))
                {
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                        Console.WriteLine($"✅ Kullanıcı eklendi: {name} ({role})");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Kullanıcı ekleme hatası: {name} - {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    await userManager.CreateAsync(user);
                    await userManager.AddToRoleAsync(user, role);
                    Console.WriteLine($"✅ Seller eklendi: {name} (Şifre admin tarafından ayarlanacak)");
                }
            }
        }
    }
}
