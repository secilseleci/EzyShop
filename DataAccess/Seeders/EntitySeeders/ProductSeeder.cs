using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders;

public static class ProductSeeder
{
    public static async Task SeedProductsAsync(ApplicationDbContext dbContext)
    {
        #region SeleciShop products
        var seleciShop = await dbContext.Shops
            .Include(s => s.Seller).ThenInclude(s => s.User)
            .FirstOrDefaultAsync(s => s.Name == "Seleci Home");

        if (seleciShop != null &&
            !await dbContext.Products.AnyAsync(p => p.ShopId == seleciShop.Id))
        {


            var electronicsCatId = Guid.Parse("dc8f3700-5fce-4c5e-a9d0-2bea740e7b19");

            dbContext.Products.Add(new Product
            {
                Name = "Camera",
                Price = 1200,
                Stock = 5,
                Color = "Black",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/Camera.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/Camera.jpg" }
                    }
            });

            dbContext.Products.Add(new Product
            {
                Name = "Drone",
                Price = 9000,
                Stock = 5,
                Color = "White",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/Drone.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/Drone.jpg" }
                    }
            });
            dbContext.Products.Add(new Product
            {
                Name = "Ear Phones",
                Price = 200,
                Stock = 5,
                Color = "White",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/Earphones.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/Earphones.jpg" }
                    }
            });
            dbContext.Products.Add(new Product
            {
                Name = "Head Phones",
                Price = 123,
                Stock = 5,
                Color = "White",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/Headphones.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/Headphones.jpg" }
                    }
            });
            dbContext.Products.Add(new Product
            {
                Name = "Ipad",
                Price = 89,
                Stock = 5,
                Color = "White",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/Ipad.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/Ipad.jpg" }
                    }
            });
            dbContext.Products.Add(new Product
            {
                Name = "JoyStick",
                Price = 567,
                Stock = 5,
                Color = "White",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/JoyStick.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/JoyStick.jpg" }
                    }
            });

            dbContext.Products.Add(new Product
            {
                Name = "JoyStick",
                Price = 299,
                Stock = 5,
                Color = "Gray",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/JoyStick.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/JoyStick.jpg" }
                    }
            });
            dbContext.Products.Add(new Product
            {
                Name = "Laptop",
                Price = 4000,
                Stock = 1,
                Color = "Gray",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/Laptop.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/Laptop.jpg" }
                    }
            });
            dbContext.Products.Add(new Product
            {
                Name = "Mobile Phone",
                Price = 7000,
                Stock = 2,
                Color = "Silver",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/MobilePhone.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/MobilePhone.jpg" }
                    }
            });

            dbContext.Products.Add(new Product
            {
                Name = "Nintendo Switch",
                Price = 6000,
                Stock = 2,
                Color = "Black",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/NintendoSwitch.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/NintendoSwitch.jpg" }
                    }
            });
            dbContext.Products.Add(new Product
            {
                Name = "Speaker",
                Price = 1000,
                Stock = 3,
                Color = "Black",
                ShopId = seleciShop.Id,
                CategoryId = electronicsCatId,
                ImageUrl = "/images/product/Speaker.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
                    {
                        new() { ImageUrl = "/images/product/Speaker.jpg" }
                    }
            });


        }
        #endregion

        #region FashionBlack products
        var FashionBlack = await dbContext.Shops
            .Include(s => s.Seller).ThenInclude(s => s.User)
            .FirstOrDefaultAsync(s => s.Name == "FashionBlack");

        if (FashionBlack != null &&
            !await dbContext.Products.AnyAsync(p => p.ShopId == FashionBlack.Id))
        {


            var fashionCatId = Guid.Parse("636cf7e1-9bef-48f1-8ba7-9f6203b6f959");

            dbContext.Products.Add(new Product
            {
                Name = "Women's Blazer",
                Price = 1299,
                Stock = 15,
                ShopId = FashionBlack.Id,
                CategoryId = fashionCatId,
                ImageUrl = "/images/product/blazer.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
            {
                new() { ImageUrl = "/images/product/blazer_1.jpg" }
            }
            });

            dbContext.Products.Add(new Product
            {
                Name = "Silk Scarf",
                Price = 499,
                Stock = 50,
                ShopId = FashionBlack.Id,
                CategoryId = fashionCatId,
                Color = "Kırmızı",
                ImageUrl = "images/product/scarf.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
            {
                new() { ImageUrl = "/images/product/scarf_1.jpg" }
            }
            });



        }

        #endregion
        #region Book Store products
        var BookShop = await dbContext.Shops
            .Include(s => s.Seller).ThenInclude(s => s.User)
            .FirstOrDefaultAsync(s => s.Name == "Book Store");

        if (BookShop != null &&
            !await dbContext.Products.AnyAsync(p => p.ShopId == BookShop.Id))
        {


            var booksCatId = Guid.Parse("279ac61d-0691-4d5a-aab0-caca11ed28c2");

            dbContext.Products.Add(new Product
            {
                Name = "Book 1",
                Price = 80,
                Stock = 3,
                ShopId = BookShop.Id,
                CategoryId = booksCatId,
                ImageUrl = "/images/product/1.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
            {
                new() { ImageUrl = "/images/product/1.jpg" }
            }
            });

            dbContext.Products.Add(new Product
            {
                Name = "Book 2",
                Price = 200,
                Stock = 2,
                ShopId = BookShop.Id,
                CategoryId = booksCatId,
                Color = "Kırmızı",
                ImageUrl = "images/product/2.jpg",
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>
            {
                new() { ImageUrl = "/images/product/2.jpg" }
            }
            });

            #endregion

        }
        await dbContext.SaveChangesAsync();
    }

}




