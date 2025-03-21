﻿using DataAccess;
using Microsoft.AspNetCore.Identity;
using Models.Entities.Concrete;
using Models.Identity;

public static class SellerApplicationSeeder
{
    public static async Task SeedSellerApplicationsAsync(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
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
                    ContactBusinessNumber = "555-999-2222",
                    Address = "Istanbul, Turkey",
                    TaxNumber = "1234567890",
                    ApplicationDate = DateTime.UtcNow,
                    Status = ApplicationStatus.Approved
                },
                new SellerApplication
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Email = "secilseller@gmail.com",
                    Name = "Seçil Seller",
                    StoreName = "SecondShop",
                    ContactNumber = "555-333-4444",
                    ContactBusinessNumber = "555-999-4444",
                    Address = "Ankara, Turkey",
                    TaxNumber = "9876543210",
                    ApplicationDate = DateTime.UtcNow,
                    Status = ApplicationStatus.Approved
                },
                new SellerApplication
                {
                    Id = Guid.NewGuid(),
                    Email = "selecisecil072@gmail.com",
                    Name = "Seçil Hanım",
                    StoreName = "HanımStore",
                    ContactNumber = "444-333-4444",
                    ContactBusinessNumber = "444-999-4344",
                    Address = "İzmir, Turkey",
                    TaxNumber = "9076543210",
                    ApplicationDate = DateTime.UtcNow,
                    Status = ApplicationStatus.Pending
                }
            };

            dbContext.SellerApplications.AddRange(applications);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("✅ Seller Applications Eklendi.");
        }

        // ✅ Approved başvuruları kontrol edip onaylıyoruz
        var approvedApplications = dbContext.SellerApplications
            .Where(a => a.Status == ApplicationStatus.Approved)
            .ToList();

        foreach (var application in approvedApplications)
        {
            var existingUser = await userManager.FindByEmailAsync(application.Email);
            if (existingUser == null)
            {
                var seller = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = application.Email,
                    Email = application.Email,
                    Name = application.Name,
                    IsSeller = true,
                    IsActive = true,
                    PhoneNumber = application.ContactNumber,
                    Address = application.Address,
                    EmailConfirmed = true
                };

                var createUserResult = await userManager.CreateAsync(seller, "Seller.123");
                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(seller, "Seller");
                    application.UserId = seller.Id;
                    dbContext.SellerApplications.Update(application);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine($"✔️ Kullanıcı oluşturuldu ve Seller yapıldı: {seller.Email}");
                }
                else
                {
                    Console.WriteLine($"❌ Kullanıcı oluşturulamadı: {application.Email}");
                }
            }
        }
    }
}
