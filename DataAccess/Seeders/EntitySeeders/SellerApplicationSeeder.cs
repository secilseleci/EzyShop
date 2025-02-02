using Models.Entities.Concrete;

namespace DataAccess.Seeders.EntitySeeders
{
    public static class SellerApplicationSeeder
    {
        public static async Task SeedSellerApplicationsAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.SellerApplications.Any())
            {
                dbContext.SellerApplications.AddRange(new List<SellerApplication>
                {
                    new SellerApplication
                    {
                        Email = "secil.seleci@gmail.com",
                        Name = "John Doe",
                        StoreName = "Doe's Electronics",
                        ContactNumber = "555-1234",
                        Address = "123 Main Street, New York, USA",
                        TaxNumber = "1234567890",
                        ApplicationDate = DateTime.UtcNow,
                        Status = ApplicationStatus.Pending
                    },
                    new SellerApplication
                    {
                        Email = "secilseleci822@gmail.com",
                        Name = "Jane Smith",
                        StoreName = "Smith's Books",
                        ContactNumber = "555-5678",
                        Address = "456 Oak Street, London, UK",
                        TaxNumber = "0987654321",
                        ApplicationDate = DateTime.UtcNow,
                        Status = ApplicationStatus.Approved
                    },
                    new SellerApplication
                    {
                        Email = "secilseleci996@gmail.com",
                        Name = "Emily Johnson",
                        StoreName = "Emily's Fashion",
                        ContactNumber = "555-9012",
                        Address = "789 Pine Avenue, Toronto, Canada",
                        TaxNumber = null,  // Opsiyonel
                        ApplicationDate = DateTime.UtcNow,
                        Status = ApplicationStatus.Rejected
                    }
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
