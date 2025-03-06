using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.User;

namespace WebUI.Controllers
{
    public class UserController : BaseController
    {
    public UserController(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper)
    : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {}


        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");
         
            var userProfileViewModel = Mapper.Map<UserProfileViewModel>(user);
 
            return View(userProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Details(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");
            model.Email = user.Email;

            Mapper.Map(model, user);

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your profile has been updated successfully!";
                return RedirectToAction("Details");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
}
}