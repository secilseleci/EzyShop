using AutoMapper;
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
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper)
   : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
    }

    #region Home Page
    public async Task<IActionResult> Index()
    {
        if (IsAuthenticated && CurrentUserId.HasValue)
        {
            var user = await UserManager.FindByIdAsync(CurrentUserId.Value.ToString());

            if (user is not null)
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

    #region Error
    [Route("Home/Error")]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var exception = feature?.Error;

        var model = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Message = exception?.Message,
            Path = feature?.Path,
            StackTrace = exception?.StackTrace
        };

        return View(model);
    }
    #endregion

    #region About & Contact
    public IActionResult About() => View();
    public IActionResult Contact() => View();
    #endregion


}