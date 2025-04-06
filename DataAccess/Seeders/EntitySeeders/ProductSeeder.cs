using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders;

public static class ProductSeeder
{
    public static async Task SeedProductsAsync(ApplicationDbContext dbContext)
    {
        var seleciShop = await dbContext.Shops
                    .Include(s => s.Seller)
                    .ThenInclude(s => s.User)
                    .FirstOrDefaultAsync(s => s.Name == "Seleci Home");

        if (seleciShop != null)
        {
            var electronicsCatId = Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19");
            if (!await dbContext.Products.AnyAsync(p => p.Name == "Smartphone"))
            {
                var product = new Product
                {
                    Name = "Smartphone",
                    Price = 8999,
                    Stock = 25,
                    Color ="Siyah",
                    ShopId = seleciShop.Id,
                    CategoryId = electronicsCatId,
                    ImageUrl = "images\\product\\galaxy_s23.jpg",
                    CreatedAt = DateTime.UtcNow
                };
                product.ProductImages.Add(new ProductImage
                {
                    ImageUrl = "images\\product\\smartphone_1.jpg"
                });

                dbContext.Products.Add(product);
            }
            var booksCatId = Guid.Parse("279ac61d-0691-4d5a-aab0-caca11ed28c2");
            if (!await dbContext.Products.AnyAsync(p => p.Name == "Design Patterns Book"))
            {
                var product = new Product
                {
                    Name = "War and Peace",
                    Price = 299,
                    Stock = 100,
                    ShopId = seleciShop.Id,
                    CategoryId = booksCatId,
                    ImageUrl = "images\\product\\war_and_peace.jpg",
                    CreatedAt = DateTime.UtcNow
                };
                product.ProductImages.Add(new ProductImage
                {
                    ImageUrl = "images\\product\\war_and_peace.jpg"
                });

                dbContext.Products.Add(product);
            }
        }
        var hanimShop = await dbContext.Shops
           .Include(s => s.Seller)
           .ThenInclude(s => s.User)
           .FirstOrDefaultAsync(s => s.Name == "Hanım Moda");

        if (hanimShop != null)
        {
            var fashionCatId = Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959");

            if (!await dbContext.Products.AnyAsync(p => p.Name == "Women's Blazer"))
            {
                var product = new Product
                {
                    Name = "Women's Blazer",
                    Price = 1299,
                    Stock = 15,
                    ShopId = hanimShop.Id,
                    CategoryId = fashionCatId,
                    Color = "Bej",
                    ImageUrl = "images\\product\\blazer.jpg",
                    CreatedAt = DateTime.UtcNow
                };
                product.ProductImages.Add(new ProductImage
                {
                    ImageUrl = "images\\product\\blazer_1.jpg"
                });

                dbContext.Products.Add(product);
            }

            if (!await dbContext.Products.AnyAsync(p => p.Name == "Silk Scarf"))
            {
                var product = new Product
                {
                    Name = "Silk Scarf",
                    Price = 499,
                    Stock = 50,
                    ShopId = hanimShop.Id,
                    CategoryId = fashionCatId,
                    Color = "Kırmızı",
                    ImageUrl = "images\\product\\scarf.jpg",
                    CreatedAt = DateTime.UtcNow
                };
                product.ProductImages.Add(new ProductImage
                {
                    ImageUrl = "images\\product\\scarf_1.jpg"
                });

                dbContext.Products.Add(product);
            }
        }


        await dbContext.SaveChangesAsync();
    }
}





