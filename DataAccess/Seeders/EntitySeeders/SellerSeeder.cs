using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders;

public static class SellerSeeder
{
    public static async Task SeedSellersAsync(ApplicationDbContext dbContext)
    {
        var user1 = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "secil.seleci@gmail.com");
        var application1 = await dbContext.SellerApplications.FirstOrDefaultAsync(a => a.Email == "secil.seleci@gmail.com");

        if (user1 != null && application1 != null)
        {
            var existingSeller1 = await dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == user1.Id);

            if (existingSeller1 == null)
            {
                dbContext.Sellers.Add(new Seller
                {
                    UserId = user1.Id,
                    SellerApplicationId = application1.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }


        var user2 = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "secilseller@gmail.com");
        var application2 = await dbContext.SellerApplications.FirstOrDefaultAsync(a => a.Email == "secilseller@gmail.com");

        if (user2 != null && application2 != null)
        {
            var existingSeller2 = await dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == user2.Id);

            if (existingSeller2 == null)
            {
                dbContext.Sellers.Add(new Seller
                {
                    UserId = user2.Id,
                    SellerApplicationId = application2.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }


        var user3 = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "selecisecil072@gmail.com");
        var application3 = await dbContext.SellerApplications.FirstOrDefaultAsync(a => a.Email == "selecisecil072@gmail.com");

        if (user3 != null && application3 != null)
        {
            var existingSeller3 = await dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == user3.Id);

            if (existingSeller3 == null)
            {
                dbContext.Sellers.Add(new Seller
                {
                    UserId = user3.Id,
                    SellerApplicationId = application3.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        
        
        await dbContext.SaveChangesAsync();
    }

}
