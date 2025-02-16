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
                        ImageUrl= "images\\product\\war_and_peace.jpg",
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
                        ImageUrl= "images\\product\\IPhone15Pro.webp",
                        Description="Apple'ın en yeni akıllı telefonu.",
                        Price=50000,
                        Color="Gri",
                        Stock=8,
                        ShopId=shop.Id,
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19")
                    },
                     new Product {
                        Id = Guid.NewGuid(),
                        Name = "JBL Kulaklık",
                        ImageUrl= "images\\product\\kulaklik.jpg",
                        Description="Kulaklık",
                        Price=50000,
                        Color="Siyah",
                        Stock=5,
                        ShopId=shop.Id,
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19")
                    },
                     new Product {
                        Id = Guid.NewGuid(),
                        Name = "erkektisort",
                        ImageUrl= "images\\product\\erkektisort.jpg",
                        Description="Erkek Tişört",
                        Price=5600,
                        Color="Gri",
                        Stock=1,
                        ShopId=shop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959")
                    },
                      new Product {
                        Id = Guid.NewGuid(),
                        Name = "Kazak",
                        ImageUrl= "images\\product\\yesilkazak.jpg",
                        Description="Erkek Triko",
                        Price=5600,
                        Color="Yeşil",
                        Stock=1,
                        ShopId=shop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959")
                    }

                });

                await dbContext.SaveChangesAsync();
                Console.WriteLine("✅ ProductSeeder başarıyla tamamlandı!");
            }
        }
    }
}
