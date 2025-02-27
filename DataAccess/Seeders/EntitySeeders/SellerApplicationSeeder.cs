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
                        UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        StoreName = "FirstShop",
                        ContactNumber = "555-111-2222",
                        ContactBusinessNumber="555-999-2222",
                        Address = "Istanbul, Turkey",
                        TaxNumber = "1234567890",
                        ApplicationDate = DateTime.UtcNow,
                        Status = ApplicationStatus.Approved,
                        ShopId = null // 🔥 Mağaza onaylanana kadar null
                    },
                    new SellerApplication
                    {
                        Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                        Email = "secilseller@gmail.com",
                        Name = "Seçil Seller",
                        UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        StoreName = "SecondShop",
                        ContactNumber = "555-333-4444",
                        ContactBusinessNumber="555-999-4444",
                        Address = "Ankara, Turkey",
                        TaxNumber = "9876543210",
                        ApplicationDate = DateTime.UtcNow,
                        Status = ApplicationStatus.Approved,
                        ShopId = null // 🔥 Mağaza onaylanana kadar null
                    }
                };

                dbContext.SellerApplications.AddRange(applications);
                await dbContext.SaveChangesAsync();
            }

            // 🔥 Sadece onaylı olan seller'lar için AppUser oluşturulmuş mu kontrol et
            var approvedApplications = dbContext.SellerApplications
                .Where(a => a.Status == ApplicationStatus.Approved)
                .ToList();

            foreach (var application in approvedApplications)
            {
                var seller = dbContext.Users.FirstOrDefault(u => u.Email == application.Email);
                if (seller != null && !seller.IsActive)
                {
                    seller.IsActive = true;
                    dbContext.Users.Update(seller);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
