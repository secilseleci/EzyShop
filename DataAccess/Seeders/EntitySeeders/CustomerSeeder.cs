using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders;

public static class CustomerSeeder
{
    public static async Task SeedCustomersAsync(ApplicationDbContext dbContext)
    {
        var derya = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "derya.karaman.ezyshop@gmail.com");
        var nur = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "nur.biral.ezyshop@gmail.com");

        if (derya != null && !await dbContext.Customers.AnyAsync(c => c.UserId == derya.Id))
        {
            dbContext.Customers.Add(new Customer
            {
                UserId = derya.Id,
                ShippingAddress = "İstanbul, Kadıköy",
                CreatedAt = DateTime.UtcNow
            });
        }

        if (nur != null && !await dbContext.Customers.AnyAsync(c => c.UserId == nur.Id))
        {
            dbContext.Customers.Add(new Customer
            {
                UserId = nur.Id,
                ShippingAddress = "İzmir, Karşıyaka",
                CreatedAt = DateTime.UtcNow
            });
        }

        await dbContext.SaveChangesAsync();
    }
}
