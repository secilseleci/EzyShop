using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders;

public static class ShopSeeder
{
    public static async Task SeedShopsAsync(ApplicationDbContext dbContext)
    {
        var seller1 = await dbContext.Sellers
     .Include(s => s.User)
     .FirstOrDefaultAsync(s => s.User.Email == "secil.seleci@gmail.com");

        if (seller1 != null && !await dbContext.Shops.AnyAsync(sh => sh.SellerId == seller1.Id))
        {
            dbContext.Shops.Add(new Shop
            {
                SellerId = seller1.Id,
                Name = "Seleci Home",
                ShopAddress = "İstanbul, Erenköy Mah. No:23",
                TaxNumber = "TR123456789",
                Status = Shop.ShopStatus.Approved,
                CreatedAt = DateTime.UtcNow
            });
        }

        var seller2 = await dbContext.Sellers
       .Include(s => s.User)
       .FirstOrDefaultAsync(s => s.User.Email == "secilseller@gmail.com");

        if (seller2 != null && !await dbContext.Shops.AnyAsync(sh => sh.SellerId == seller2.Id))
        {
            dbContext.Shops.Add(new Shop
            {
                SellerId = seller2.Id,
                Name = "Hanım Moda",
                ShopAddress = "Ankara, Kızılay Cd. No:12",
                TaxNumber = "TR987654321",
                Status = Shop.ShopStatus.Approved,
                CreatedAt = DateTime.UtcNow
            });
        }
        await dbContext.SaveChangesAsync();

    }
}
