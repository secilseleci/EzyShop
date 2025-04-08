using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

public static class SellerApplicationSeeder
{
    public static async Task SeedSellerApplicationsAsync(ApplicationDbContext dbContext )
    {
        var email1 = "secil.seleci@gmail.com";
        
        if (!await dbContext.SellerApplications.AnyAsync(sa => sa.Email == email1))
        {
            dbContext.SellerApplications.Add(new SellerApplication

            {
                Email = email1,
                Name = "Seçil",
                Surname="Seleci",
                ShopName = "Seleci home",
                ContactBusinessNumber = "212-999-2222",
                TaxNumber = "1234",
                CreatedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Approved,
                ShopAddress = "İstanbul/Sarıyer"

            });
        }


        var email2 = "secilseller@gmail.com";

        if (!await dbContext.SellerApplications.AnyAsync(sa => sa.Email == email2))
        {
            dbContext.SellerApplications.Add(new SellerApplication

            {
                Email = email2,
                Name = "Seçil",
                Surname ="Hanım",
                ShopName = "Hanım Moda",
                ContactBusinessNumber = "212-999-3333",
                TaxNumber = "1236",
                CreatedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Approved,
                ShopAddress = "Ankara/Çankaya"
            });
        }

        await dbContext.SaveChangesAsync();

    }
}
