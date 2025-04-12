using AutoMapper;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;

public class HomeController : BaseController
{

    public HomeController(
     ICurrentUserService currentUserService, 
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper)
   : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
    }


    #region Home Page

    public async Task<IActionResult> Index()
    {
        if (CurrentUserService.IsAuthenticated && CurrentUserService.UserId.HasValue)
        {
            var userId = CurrentUserService.UserId.Value.ToString();
            var user = await UserManager.FindByIdAsync(userId);

            if (user != null)
            {
                var roles = await UserManager.GetRolesAsync(user);

                if (roles.Contains("Seller"))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Seller" });
                }

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }
        }


        return View();  
    }


    #endregion
     

    #region About & Contact
    public IActionResult About() => View();
    public IActionResult Contact() => View();
    #endregion


}