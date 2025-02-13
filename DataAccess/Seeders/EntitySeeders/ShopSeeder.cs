using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using Models.Identity;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class ShopSeeder
    {
        public static async Task SeedShopsAsync(ApplicationDbContext dbContext)
        {
            var approvedSellers = dbContext.Users
                .Where(u => u.IsSeller && u.IsActive)
                .ToList();

            if (!approvedSellers.Any())
            {
                Console.WriteLine("❌ Hata: Onaylı Seller bulunamadı! Önce Admin tarafından onay verilmesi gerekiyor.");
                return;
            }

            foreach (var seller in approvedSellers)
            {
                var existingShop = dbContext.Shops
                    .AsNoTracking()
                    .FirstOrDefault(s => s.SellerId == seller.Id);

                if (existingShop == null)
                {
                    var newShop = new Shop
                    {
                        Id = Guid.NewGuid(), // 🔥 Her mağaza için benzersiz ID
                        Name = seller.StoreName ?? $"{seller.Name}'s Shop",
                        SellerId = seller.Id,
                        ContactNumber = seller.ContactNumber ?? "000-000-0000",
                        Address = seller.Address ?? "Belirtilmemiş",
                        TaxNumber = seller.TaxNumber,
                        IsActive = true,
                        Status = "Approved"
                    };

                    dbContext.Shops.Add(newShop);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine($"✅ {seller.Name} için mağaza eklendi: {newShop.Name}");
                }
                else
                {
                    Console.WriteLine($"⚠️ Uyarı: {seller.Name} zaten bir mağazaya sahip, tekrar eklenmedi.");
                }
            }
        }
    }
}
