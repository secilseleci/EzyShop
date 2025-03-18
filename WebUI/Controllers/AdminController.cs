using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.User;
using System.Linq.Expressions;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    { private readonly ISellerApplicationService _sellerApplicationService;
        private readonly IEmailService _emailService;


        public AdminController(

            ISellerApplicationService  sellerApplicationService,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            IEmailService emailService,
            IMapper mapper)
            : base(userManager, roleManager, signInManager, webHostEnvironment, mapper  )
        {
            _sellerApplicationService = sellerApplicationService; 
            _emailService = emailService;

        }


        #region USER
        [HttpGet]
        public  IActionResult  ListUsers()
        {
            var users = UserManager.Users.ToList();
            return View();
        }


        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await UserManager.CreateAsync(user, "DefaultPassword123!");
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to create user.");
                return View(model);
            }

            if (model.Role != null )
            {
                var roleResult = await UserManager.AddToRoleAsync(user, model.Role);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to assign role.");
                    return View(model);
                }
            }

            TempData["SuccessMessage"] = "User successfully added.";
            return RedirectToAction("ListUsers");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Kullanıcı başarıyla silindi." });
            }
            return Json(new { success = false, message = "Kullanıcı silinemedi." });
        }
        

        [HttpPost]
        public async Task<IActionResult> ResetUserPassword(string userId)
        {

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            string newPassword = GenerateRandomPassword();

            var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            var result = await UserManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (!result.Succeeded)
            {
                return Json(new { success = false, message = "Password reset failed" });
            }

            string subject = "Şifre Sıfırlama Bilgilendirmesi";
            string body = $"Merhaba {user.Name},\n\nŞifreniz başarıyla sıfırlandı. Yeni şifreniz: {newPassword}\n\nLütfen giriş yaptıktan sonra şifrenizi değiştirin.";

            bool emailSent = await _emailService.SendEmailAsync(user.Email, subject, body);
            if (!emailSent)
            {
                return Json(new { success = false, message = "Password reset successful, but email could not be sent." });
            }

            return Json(new { success = true, message = "Password reset successful. Email has been sent." });
        }

        private string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 10) + "Aa*";
        }

    

        //Api Calls Get All Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await UserManager.Users.ToListAsync();

            var AdminUser =   users.FirstOrDefault(user =>
               UserManager.IsInRoleAsync(user, "Admin").Result);
            if (AdminUser != null)
            {
                users.Remove(AdminUser);
                users.Insert(0, AdminUser);
            }

            var userDataWithRoles = users.Select(user => new
            {
                user.Name,
                user.Email,
                user.Id,
                Roles = UserManager.GetRolesAsync(user).Result,
            }).ToList();

            return Json(new { data = userDataWithRoles });
        }
        #endregion
 

        #region SellerApplication

        [HttpGet]
        public async Task<IActionResult> ListSellerApplications()
        {
             var result = await _sellerApplicationService.GetAllSellerApplicationsAsync(a => true);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSellerApplications(string status = "all")
        {
            Expression<Func<SellerApplication, bool>> filter = status.ToLower() switch
            {
                "approved" => a => a.Status == ApplicationStatus.Approved,
                "rejected" => a => a.Status == ApplicationStatus.Rejected,
                "pending" => a => a.Status == ApplicationStatus.Pending,
                _ => a => true
            };

            var result = await _sellerApplicationService.GetAllSellerApplicationsAsync(filter);

            if (!result.Success || result.Data == null || !result.Data.Any())
            {
                return PartialView("_SellerApplicationsTable", new List<SellerApplication>()); // Boş liste döndür
            }

            return PartialView("_SellerApplicationsTable", result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveSeller(Guid id)
        {
            var result = await _sellerApplicationService.ApproveSellerAsync(id);
            if (result.Success)
            {
                return Json(new { success = true, message = result.Message });
            }
            else
            {
                return Json(new { success = false, message = result.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RejectSeller(Guid id)
        {
            var result = await _sellerApplicationService.RejectSellerAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction("ListSellerApplications");
        }
        #endregion

    }
}