using Models.Entities.Concrete;
using Models.Identity;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class SellerApplicationSeeder
    {
        public static async Task SeedSellerApplicationsAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.SellerApplications.Any())
            {
                var applications = new List<SellerApplication>
                {
                    new SellerApplication
                    {
                        Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                        Email = "secil.seleci@gmail.com",
                        Name = "Seçil Seleci",
                        StoreName = "FirstShop",
                        ContactNumber = "555-111-2222",
                        Address = "Istanbul, Turkey",
                        TaxNumber = "1234567890",
                        ApplicationDate = DateTime.UtcNow,
                        Status = ApplicationStatus.Approved
                    }, new SellerApplication
                    {
                        Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                        Email = "secilseller@gmail.com",
                        Name = "Seçil Seller",
                        StoreName = "SecondShop",
                        ContactNumber = "555-333-4444",
                        Address = "Ankara, Turkey",
                        TaxNumber = "9876543210",
                        ApplicationDate = DateTime.UtcNow,
                        Status = ApplicationStatus.Approved
                    }
                };

                dbContext.SellerApplications.AddRange(applications);
                await dbContext.SaveChangesAsync();
             }

            // 🔥 Approved seller’ların IsActive durumunu güncelle
            var approvedApplications = dbContext.SellerApplications
                .Where(a => a.Status == ApplicationStatus.Approved)
                .ToList();

            foreach (var application in approvedApplications)
            {
                var seller = dbContext.Users.FirstOrDefault(u => u.Email == application.Email);
                if (seller != null && !seller.IsActive)
                {
                    seller.IsActive = true; // 🔥 Onaylı başvuruya sahip seller’ları aktif yap
                    dbContext.Users.Update(seller);
                }
            }

            await dbContext.SaveChangesAsync();
         }
    }
}
