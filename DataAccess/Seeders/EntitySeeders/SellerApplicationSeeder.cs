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
                ShopName = "Seleci Home",
                ContactBusinessNumber = "212-999-2222",
                TaxNumber = "111",
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
                Name = "Fatma",
                Surname ="Kara",
                ShopName = "FashionBlack",
                ContactBusinessNumber = "212-999-3333",
                TaxNumber = "222",
                CreatedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Approved,
                ShopAddress = "Ankara/Çankaya"
            });
        }


        var email3 ="selecisecil072@gmail.com";

        if (!await dbContext.SellerApplications.AnyAsync(sa => sa.Email == email3))
        {
            dbContext.SellerApplications.Add(new SellerApplication

            {
                Email = email3,
                Name = "Mete",
                Surname = "Bakırcı",
                ShopName = "Book Store",
                ContactBusinessNumber = "212-999-3333",
                TaxNumber = "333",
                CreatedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Approved,
                ShopAddress = "İzmir/Konak"
            });
        }
        await dbContext.SaveChangesAsync();

    }
}
