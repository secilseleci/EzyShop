using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Shop;

namespace WebUI.Controllers;
 
public class ShopController : BaseController
{
    private readonly IShopService _shopService;

    public ShopController(
          ICurrentUserService currentUserService,
          IShopService shopService,
          UserManager<AppUser> userManager,
          RoleManager<AppRole> roleManager,
          SignInManager<AppUser> signInManager,
          IWebHostEnvironment webHostEnvironment,
          IMapper mapper)
    : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
       
    {
        _shopService = shopService;
    }
    #region Shop Page
    public IActionResult Shop()
    {
        return View();
    }
    public async Task<IActionResult> RenderProductCard(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
    {
        return ViewComponent("ProductCard", new { name, category, color, minPrice, maxPrice });
    }
    #endregion    
    
    //[HttpGet]
    //public async Task<IActionResult> Index()
    //{
    //    var user = await GetCurrentUserAsync();
    //    if (user == null) return RedirectToAction("Login", "Account");

    //    var result = await _shopService.GetShopBySellerIdAsync(user.Id);

    //    if (!result.Success || result.Data == null)
    //    {
    //        TempData["ErrorMessage"] = "No shop found. Please contact admin.";
    //        return View(null);
    //    }

    //    return View(result.Data);
    //}

  
   
}
