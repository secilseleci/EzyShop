using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using Models.Identity;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class ShopSeeder
    {
        public static async Task SeedShopsAsync(ApplicationDbContext dbContext)
        {
            var approvedApplications = dbContext.SellerApplications
                .Where(a => a.Status == ApplicationStatus.Approved && a.ShopId == null)
                .ToList();

            if (!approvedApplications.Any())
            {
                Console.WriteLine("⚠️ Onaylı ama mağazası olmayan seller başvurusu bulunamadı.");
                return;
            }

            foreach (var application in approvedApplications)
            {
                var seller = dbContext.Users.FirstOrDefault(u => u.Id == application.UserId);
                if (seller == null)
                {
                    Console.WriteLine($"❌ Hata: {application.Email} için Seller bulunamadı!");
                    continue;
                }

                var newShop = new Shop
                {
                    Id = Guid.NewGuid(), // 🔥 Her mağaza için benzersiz ID
                    Name = application.StoreName,
                    SellerId = seller.Id,
                    ContactNumber = application.ContactNumber,
                    Address = application.Address,
                    TaxNumber = application.TaxNumber,
                    IsActive = true,
                    Status = "Approved"
                };

                dbContext.Shops.Add(newShop);
                await dbContext.SaveChangesAsync();

                // 🔥 SellerApplication'a ShopId ekle
                application.ShopId = newShop.Id;
                dbContext.SellerApplications.Update(application);
                await dbContext.SaveChangesAsync();

                Console.WriteLine($"✅ {seller.Name} için mağaza eklendi: {newShop.Name}");
            }
        }
    }
}
