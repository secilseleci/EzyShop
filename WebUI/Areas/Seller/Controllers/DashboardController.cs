using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Seller.Controllers;

[Area("Seller")]
[Route("Seller/[controller]/[action]")]
[Authorize(Roles = "Seller")]
public class DashboardController : BaseController
{
    private readonly ISellerService _sellerService;


    public DashboardController
    (
        ICurrentUserService currentUserService,
        ISellerService sellerService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
        : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _sellerService = sellerService;
    }
    public IActionResult Index()
    {
        return View();
     }

    

   
}
