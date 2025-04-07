using Microsoft.AspNetCore.Identity;
using Models.Identity;

namespace DataAccess.Seeders.IdentitySeeders;

public static class AppUserSeeder
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        // ✅ Admin

        var adminEmail = "admin@ezyshop.com";
        var adminUserName = "Admin";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    Name = "Admin",
                    Surname = "User",
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "05550000000",
                };
            
            var result = await userManager.CreateAsync(newAdmin, "Admin123*");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }

        // ✅ Customer1
        var customer1Email = "derya.karaman.ezyshop@gmail.com";
        if (await userManager.FindByEmailAsync(customer1Email) == null)
        {
            var customer = new AppUser
            {
                Name = "Derya",
                Surname = "Karaman",
                UserName = "Derya Karaman",
                Email = customer1Email,
                EmailConfirmed = true,
                PhoneNumber = "05551112233",
               
            };

            var resultCustomer1 = await userManager.CreateAsync(customer, "Customer.123");

            if (resultCustomer1.Succeeded)
            {
                await userManager.AddToRoleAsync(customer, "Customer");
            }
                 
        }

        // ✅ Customer2

        var customer2Email = "nur.biral.ezyshop@gmail.com";
        if (await userManager.FindByEmailAsync(customer2Email) == null)
            {
                var customer = new AppUser
                {
                    Name = "Nur",
                    Surname = "Biral",
                    UserName = "Nur Biral",
                    Email = customer2Email,
                    EmailConfirmed = true,
                    PhoneNumber = "05551112244",
                    
                };

                var resultCustomer2 = await userManager.CreateAsync(customer, "Customer.123");

                if (resultCustomer2.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, "Customer");
                }

            }

        // ✅ Seller1
        var seller1Email = "secil.seleci@gmail.com";
        if(await userManager.FindByEmailAsync(seller1Email) == null) 
        {
            var seller = new AppUser
            {
                Name = "Seçil",
                Surname = "Seleci",
                UserName = "Seçil Seleci",
                Email = seller1Email,
                EmailConfirmed = true,
                PhoneNumber = "05534102506",
                
            };
            var resultSeller1 = await userManager.CreateAsync(seller, "Seller.123");

            if (resultSeller1.Succeeded)
            {
                await userManager.AddToRoleAsync(seller, "Seller");
            }
        }


        // ✅ Seller2
        var seller2Email = "secilseller@gmail.com";

        if (await userManager.FindByEmailAsync(seller2Email) == null)
        {
            var seller = new AppUser
            {
                Name = "Seçil",
                Surname = "Hanım",
                UserName = "Seçil Hanım",
                Email = seller2Email,
                EmailConfirmed = true,
                PhoneNumber = "05555552506",
              
            };
            var resultSeller2 = await userManager.CreateAsync(seller, "Seller.123");

            if (resultSeller2.Succeeded)
            {
                await userManager.AddToRoleAsync(seller, "Seller");
            }
        }


    }
}

