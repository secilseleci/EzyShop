using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ShopController : BaseController
{
    private readonly IShopService _shopService;
    public ShopController(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IEmailService emailService,
        IMapper mapper,
        IShopService shopService)
        : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _shopService = shopService;
    }

    #region List Shops
    public IActionResult Pending()
    {
        ViewBag.Status = "Pending";
        return View("ShopList");
    }
    public IActionResult Active()
    {
        ViewBag.Status = "Active";
        return View("ShopList");
    }
    public IActionResult Inactive()
    {
        ViewBag.Status = "Inactive";
        return View("ShopList");
    }
    public IActionResult Deleted()
    {
        ViewBag.Status = "Deleted";
        return View("ShopList");
    }
    #endregion

    #region Shop Detail
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, string status)
    {
        var result = await _shopService.GetShopDetailsAsync(id);

        if (!result.Success)
            return NotFound(new { success = false, message = result.Message });
         
        return PartialView("_ShopDetailsPartial", result.Data);
    }
    #endregion
}
