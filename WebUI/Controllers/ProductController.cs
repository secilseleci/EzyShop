using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;

public class ProductController: BaseController
{
public ProductController(
 UserManager<AppUser> userManager,
 RoleManager<AppRole> roleManager,
 SignInManager<AppUser> signInManager,
 IWebHostEnvironment webHostEnvironment,
 IMapper mapper)
: base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
{
}

 
    public IActionResult Index()
    {
        return View();
    }
}
