using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
 
        public AdminController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
            : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
        }

        [HttpGet]
        public  IActionResult  ListUsers()
        {
            var users = UserManager.Users.ToList();
            return View();
        }

        //[HttpGet]
        //public IActionResult EditRole()
        //{
        //    return View();

        //}
        //[HttpPost]
        //public async Task<IActionResult> EditRoles(EditRoleViewModel model)
        //{
        //    var user=await UserManager.FindByIdAsync(model.UserId);
        //    if (user == null)
        //    {
        //        return NotFound("User NotFound");
        //    }

        //    var currentRoles = await UserManager.GetRolesAsync(user);
        //    await UserManager.RemoveFromRolesAsync(user, currentRoles);

        //    var result = await UserManager.AddToRoleAsync(user, model.NewRole);
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Rol değiştirilemedi.");
        //        return View(model);
        //    }
        //    TempData["SuccessMessage"] = "Role başarıyla güncellendi.";
        //    return RedirectToAction("ManageUsers");
        //}

        #region Api Calls Get All Users
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
        
        #region Delete Users
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
        #endregion
        
        #region Create Users
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

            // Yeni kullanıcı oluştur
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await UserManager.CreateAsync(user, "DefaultPassword123!"); // Varsayılan şifre
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to create user.");
                return View(model);
            }

            // Kullanıcıya rol ekle
            if (model.Roles != null && model.Roles.Any())
            {
                var roleResult = await UserManager.AddToRoleAsync(user, model.Roles.First());
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to assign role.");
                    return View(model);
                }
            }

            TempData["SuccessMessage"] = "User successfully added.";
            return RedirectToAction("ListUsers");
        }
        #endregion

       
    }
}