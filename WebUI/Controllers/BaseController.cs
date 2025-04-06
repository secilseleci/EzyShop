using AutoMapper;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Identity;
namespace WebUI.Controllers;
public class BaseController : Controller
{
    protected UserManager<AppUser> UserManager { get; }
    protected RoleManager<AppRole> RoleManager { get; }
    protected SignInManager<AppUser> SignInManager { get; }
    protected ICurrentUserService CurrentUserService { get; }
    protected IWebHostEnvironment WebHostEnvironment { get; }
    protected IMapper Mapper { get; }

    public BaseController(
     ICurrentUserService currentUserService,
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper)
    {
        CurrentUserService = currentUserService;
        UserManager = userManager;
        RoleManager = roleManager;
        SignInManager = signInManager;
        WebHostEnvironment = webHostEnvironment;
        Mapper = mapper;
    }



    protected Guid? GetUserId() => CurrentUserService.UserId;

    protected async Task<AppUser?> GetCurrentUserAsync()
    {
        

        if (!CurrentUserService.UserId.HasValue)
            return null;

        return await UserManager.Users
            .FirstOrDefaultAsync(u => u.Id == CurrentUserService.UserId.Value);
    }
}
