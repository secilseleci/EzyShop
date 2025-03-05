using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class ProductSeeder
    {
        public static async Task SeedProductsAsync(ApplicationDbContext dbContext)
        {
            var firstShop = dbContext.Shops.AsNoTracking().FirstOrDefault(s => s.Name == "FirstShop");
            var secondShop = dbContext.Shops.AsNoTracking().FirstOrDefault(s => s.Name == "SecondShop");

            if (firstShop == null || secondShop == null)
            {
                Console.WriteLine("❌ Hata: Mağazalar bulunamadı! Ürünler eklenemedi.");
                return;
            }

            var existingProductNames = await dbContext.Products
                 .AsNoTracking()
                 .Select(p => p.Name)
                 .ToListAsync();

            var productsToAdd = new List<Product> { 


                     new Product {
                        Id = Guid.NewGuid(),
                        Name = "Tişört",
                        ImageUrl= "images\\product\\erkektisort.jpg",
                        Description="Erkek Tişört",
                        Price=50,
                        Color="Gri",
                        Stock=1,
                        ShopId=firstShop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959")
                    },
                      new Product {
                        Id = Guid.NewGuid(),
                        Name = "Kazak",
                        ImageUrl= "images\\product\\beyazkazak.jpg",
                        Description="Erkek Triko",
                        Price=100,
                        Color="Beyaz",
                        Stock=1,
                        ShopId=firstShop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959")
                    },

                       new Product {
                        Id = Guid.NewGuid(),
                        Name = "Samsung Galaxy S23",
                        ImageUrl= "images\\product\\galaxy_s23.jpg",
                        Description="Samsung'un en son amiral gemisi telefonu.",
                        Price=40000,
                        Color="Siyah",
                        Stock=5,
                        ShopId=secondShop.Id,  
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19")
                    },
                    //new Product {
                    //    Id = Guid.NewGuid(),
                    //    Name = "Macbook Air M2",
                    //    ImageUrl= "images\\product\\macbook_air_m2.jpg",
                    //    Description="Apple'ın yeni nesil dizüstü bilgisayarı.",
                    //    Price=60000,
                    //    Color="Gümüş",
                    //    Stock=3,
                    //    ShopId=secondShop.Id,
                    //    CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19")
                    //},
                    new Product {
                        Id = Guid.NewGuid(),
                        Name = "iPhone 15 Pro",
                        ImageUrl= "images\\product\\IPhone15Pro.webp",
                        Description="Apple'ın en yeni akıllı telefonu.",
                        Price=50000,
                        Color="Gri",
                        Stock=8,
                        ShopId=secondShop.Id,
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19")
                    },

                     new Product {
                        Id = Guid.NewGuid(),
                        Name = "JBL Kulaklık",
                        ImageUrl= "images\\product\\kulaklik.jpg",
                        Description="Kulaklık",
                        Price=9000,
                        Color="Siyah",
                        Stock=5,
                        ShopId=secondShop.Id,
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19")
                     }
            };
            var newProducts = productsToAdd
                .Where(p => !existingProductNames.Contains(p.Name)) // Zaten eklenmiş olanları atla
                .ToList();

            if (newProducts.Any())
            {
                dbContext.Products.AddRange(newProducts);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("✅ Yeni ürünler eklendi!");
            }
            else
            {
                Console.WriteLine("⚠️ Tüm ürünler zaten kayıtlı, yeni ekleme yapılmadı.");
            }
        }

    }
}
