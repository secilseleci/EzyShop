using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Seller.Controllers;

[Area("Seller")]
[Authorize(Roles = "Seller")]
public class DashboardController : BaseController
{

    public DashboardController
    (
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,

        IMapper mapper)
        : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {

    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
      
}
