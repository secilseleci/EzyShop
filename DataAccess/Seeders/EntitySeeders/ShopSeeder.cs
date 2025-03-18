using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class ShopSeeder
    {
        public static async Task SeedShopsAsync(ApplicationDbContext dbContext)
        {
            var approvedSellers = dbContext.Users
    .Where(u => u.IsSeller && u.IsActive) // Shop null kontrolü kaldırıldı çünkü güvenilir değil
    .ToList();

            foreach (var seller in approvedSellers)
            {
                var newShop = new Shop
                {
                    Id = Guid.NewGuid(),
                    Name = $"{seller.Name}'s Shop",
                    SellerId = seller.Id,
                    BusinessPhoneNumber = seller.PhoneNumber ?? "0000000000",
                    Address = seller.Address ?? "Unknown Address",
                    TaxNumber = null,
                    IsActive = true,
                    Status = Shop.ShopStatus.Approved
                };

                dbContext.Shops.Add(newShop);
                await dbContext.SaveChangesAsync();

                seller.Shop = newShop;
                dbContext.Users.Update(seller);
                await dbContext.SaveChangesAsync();

                Console.WriteLine($"✅ {seller.Name} için mağaza eklendi: {newShop.Name}");
            }
        }
    }
}
