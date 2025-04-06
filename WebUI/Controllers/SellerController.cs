using AutoMapper;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Models.Identity;

namespace WebUI.Controllers;

public class SellerController : BaseController
{
    public SellerController(UserManager<AppUser> userManager,
         ICurrentUserService currentUserService,
         RoleManager<AppRole> roleManager,
         SignInManager<AppUser> signInManager,
         IWebHostEnvironment webHostEnvironment,
         IMapper mapper)
         : base(currentUserService,userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {   
    }
}
