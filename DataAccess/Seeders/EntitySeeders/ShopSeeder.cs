using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using Models.Identity;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class ShopSeeder
    {
        public static async Task SeedShopsAsync(ApplicationDbContext dbContext)
        {
            // 1️⃣ **Seller olan ama mağazası olmayan AppUser'ları çekiyoruz**
            var approvedSellers = dbContext.Users
                .Where(u => u.IsSeller && u.IsActive && u.Shop == null)
                .ToList();

            if (!approvedSellers.Any())
            {
                Console.WriteLine("⚠️ Aktif ve mağazası olmayan onaylı satıcı bulunamadı.");
                return;
            }

            foreach (var seller in approvedSellers)
            {
                // 2️⃣ **Shop nesnesini oluştur ve AppUser ile ilişkilendir**
                var newShop = new Shop
                {
                    Id = Guid.NewGuid(), // 🔥 Her mağaza için benzersiz ID
                    Name = seller.StoreName ?? $"{seller.Name}'s Shop", // Varsayılan olarak kullanıcı adını kullan
                    SellerId = seller.Id,
                    BusinessPhoneNumber = seller.PhoneNumber ?? "0000000000", // Varsayılan telefon numarası
                    Address = seller.Address ?? "Unknown Address", // Varsayılan adres
                    TaxNumber = seller.TaxNumber, // Vergi numarası seller'dan geliyor
                    IsActive = true,  // Mağaza varsayılan olarak aktif açılır
                    Status = Shop.ShopStatus.Approved  // 🔥 Varsayılan olarak **Approved** başlatıyoruz
                };

                dbContext.Shops.Add(newShop);
                await dbContext.SaveChangesAsync();

                // 3️⃣ **Seller'ın Shop nesnesini güncelle**
                seller.Shop = newShop;
                dbContext.Users.Update(seller);
                await dbContext.SaveChangesAsync();

                Console.WriteLine($"✅ {seller.Name} için mağaza eklendi: {newShop.Name}");
            }
        }
    }
}
