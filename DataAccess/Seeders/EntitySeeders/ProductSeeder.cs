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
                Console.WriteLine("⚠️ Shop bulunamadı, lütfen ShopSeeder'ı çalıştırın!");
                return;  // Hata yerine, işlem yapmadan çık
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
                        Description="Tişört",
                        Price=50,
                        Color="Gri",
                        Stock=10,
                        ShopId=firstShop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959"),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                    },
                      new Product {
                        Id = Guid.NewGuid(),
                        Name = "Gömlek",
                        ImageUrl= "images\\product\\hakigomlek.jpg",
                        Description="Erkek Gömlek",
                        Price=500,
                        Color="Haki",
                        Stock=10,
                        ShopId=firstShop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959"),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                    },
                          new Product {
                        Id = Guid.NewGuid(),
                        Name = "Gömlek",
                        ImageUrl= "images\\product\\bejgomlek.jpg",
                        Description="Gömlek",
                        Price=330,
                        Color="Bej",
                        Stock=10,
                        ShopId=firstShop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959"),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                    },
                      new Product {
                        Id = Guid.NewGuid(),
                        Name = "sweatshirt",
                        ImageUrl= "images\\product\\sweatshirt.jpg",
                        Description="sweatshirt",
                        Price=330,
                        Color="Siyah",
                        Stock=1,
                        ShopId=firstShop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959"),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                      },
                      new Product {
                        Id = Guid.NewGuid(),
                        Name = "Kazak",
                        ImageUrl= "images\\product\\beyazkazak.jpg",
                        Description="Triko",
                        Price=100,
                        Color="Beyaz",
                        Stock=1,
                        ShopId=firstShop.Id,
                        CategoryId=Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959"),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
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
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19"),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
                    },
                   
                    new Product {
                        Id = Guid.NewGuid(),
                        Name = "iPhone 15 Pro",
                        ImageUrl= "images\\product\\IPhone15Pro.webp",
                        Description="Apple'ın en yeni akıllı telefonu.",
                        Price=50000,
                        Color="Gri",
                        Stock=8,
                        ShopId=secondShop.Id,
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19"),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,
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
                        CategoryId=Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19"),
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true, 
                        IsDeleted = false,
                     }
            };
            var newProducts = productsToAdd
                .Where(p => !existingProductNames.Contains(p.Name)) 
                .ToList();

            if (newProducts.Any())
            {
                dbContext.Products.AddRange(newProducts);
                await dbContext.SaveChangesAsync();
            }
          
        }

    }
}
