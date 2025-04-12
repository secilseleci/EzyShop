using AutoMapper;
using Core.Security;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using System.Diagnostics;
using WebUI.Models;

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
    [Route("Home/Error")]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var exception = feature?.Error;

        var model = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        return View(model);
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