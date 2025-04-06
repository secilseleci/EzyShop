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
    public IActionResult Index()
    {
        return View();
    }
    #endregion

    

    #region Details
    public IActionResult Details()
    {
        return View();
    }
    #endregion

    #region About & Contact
    public IActionResult About() => View();
    public IActionResult Contact() => View();
    #endregion


}