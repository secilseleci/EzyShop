using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class OrderSeeder
    {
        public static async Task SeedOrdersAsync(ApplicationDbContext dbContext)
        {
            var firstShop = dbContext.Shops.AsNoTracking().FirstOrDefault(s => s.Name == "FirstShop");
            var secondShop = dbContext.Shops.AsNoTracking().FirstOrDefault(s => s.Name == "SecondShop");

            var firstCustomer = dbContext.Users.AsNoTracking().FirstOrDefault(s => s.Name == "Derya Karaman");
            var secondCustomer = dbContext.Users.AsNoTracking().FirstOrDefault(s => s.Name == "Nur Biral");
            
            var firstShopProducts = dbContext.Products.Where(p => p.ShopId == firstShop.Id).ToList();
            var secondShopProducts = dbContext.Products.Where(p => p.ShopId == secondShop.Id).ToList();


            if (!dbContext.Orders.Any())
            {
                var ordersToAdd = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    OrderCode = "ORD-20240315093045-2748",
                    CustomerId = firstCustomer.Id,
                    ShopId = firstShop.Id,
                    TotalAmount = 50,
                    PaymentMethod = PaymentMethod.BankTransfer,
                    Status = Status.Pending,
                    ShippingTrackingNumber = null,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            ProductId = firstShopProducts[0].Id,
                            ProductName = firstShopProducts[0].Name,
                            ProductPrice = firstShopProducts[0].Price,
                            Color = firstShopProducts[0].Color,
                            ImageUrl = firstShopProducts[0].ImageUrl,
                            Count = 1
                        }
                    }
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    OrderCode = "ORD-20240315101530-5823",
                    CustomerId = firstCustomer.Id,
                    ShopId = secondShop.Id,
                    TotalAmount = 500,
                    PaymentMethod = PaymentMethod.CreditCard,
                    Status = Status.Delivered,
                    ShippingTrackingNumber = "TRK-20240315101530-5823",
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    UpdatedDate = DateTime.UtcNow.AddDays(-1),
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            ProductId = secondShopProducts[1].Id,
                            ProductName = secondShopProducts[1].Name,
                            ProductPrice = secondShopProducts[1].Price,
                            Color = secondShopProducts[1].Color,
                            ImageUrl = secondShopProducts[1].ImageUrl,
                            Count = 2
                        }
                    }
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    OrderCode = "ORD-20240315111240-3921",
                    CustomerId = firstCustomer.Id,
                    ShopId = firstShop.Id,
                    TotalAmount = 330,
                    PaymentMethod = PaymentMethod.CashOnDelivery,
                    Status = Status.Shipped,
                    ShippingTrackingNumber = "TRK-20240315111240-3921",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    UpdatedDate = DateTime.UtcNow.AddHours(-6),
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            ProductId = firstShopProducts[2].Id,
                            ProductName = firstShopProducts[2].Name,
                            ProductPrice = firstShopProducts[2].Price,
                            Color = firstShopProducts[2].Color,
                            ImageUrl = firstShopProducts[2].ImageUrl,
                            Count = 1
                        }
                    }
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    OrderCode = "ORD-20240315123030-7456",
                    CustomerId = secondCustomer.Id,
                    ShopId = secondShop.Id,
                    TotalAmount = 9000,
                    PaymentMethod = PaymentMethod.BankTransfer,
                    Status = Status.Cancelled,
                    ShippingTrackingNumber = null,
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    UpdatedDate = DateTime.UtcNow.AddDays(-5),
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            ProductId = secondShopProducts[2].Id,
                            ProductName = secondShopProducts[2].Name,
                            ProductPrice = secondShopProducts[2].Price,
                            Color = secondShopProducts[2].Color,
                            ImageUrl = secondShopProducts[2].ImageUrl,
                            Count = 1
                        }
                    }
                }

            };



                dbContext.Orders.AddRange(ordersToAdd);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    }
  
