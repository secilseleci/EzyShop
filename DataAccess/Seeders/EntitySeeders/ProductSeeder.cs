using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class ProductSeeder
    {
        public static async Task SeedProductsAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                var shop = dbContext.Shops
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Name == "FirstShop");

                if (shop == null)
                {
                    Console.WriteLine("❌ Hata: Seçil Seleci'nin mağazası bulunamadı! Ürünler eklenemedi.");
                    return;
                }

                dbContext.Products.AddRange(new List<Product>
                {
                    new Product {
                        Id = Guid.NewGuid(),
                        Name = "Savaş ve Barış",
                        ImageUrl= "images/product/war_and_peace.jpg",
                        Description="Lev Tolstoy'un klasik romanı.",
                        Price=500,
                        Color="Beyaz",
                        Stock=15,
                        ShopId=shop.Id,
                        CategoryId=Guid.Parse("279ac61d-0691-4d5a-aab0-caca11ed28c2")
                    },
                    new Product {
                        Id = Guid.NewGuid(),
                        Name = "iPhone 15 Pro",
                        ImageUrl= "images/product/iphone15pro.webp",
                        Description="Apple'ın en yeni akıllı telefonu.",
                        Price=50000,
                        Color="Gri",
                        Stock=8,
                        ShopId=shop.Id,
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19")
                    }
                });

                await dbContext.SaveChangesAsync();
                Console.WriteLine("✅ ProductSeeder başarıyla tamamlandı!");
            }
        }
    }
}
