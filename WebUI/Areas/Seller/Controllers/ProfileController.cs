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
public class ProfileController:BaseController
{
    private readonly ISellerService _sellerService;

    public ProfileController
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
    public async Task<IActionResult> Index()
    {
        if (!CurrentUserService.UserId.HasValue)
            return Unauthorized();
        
        var result = await _sellerService.GetSellerByUserId(CurrentUserService.UserId.Value);

        if (!result.Success)
            return NotFound(result.Message);

        return View(result.Data);
    }

}
