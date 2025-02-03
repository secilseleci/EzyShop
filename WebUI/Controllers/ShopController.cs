using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Seller")]

    public class ShopController:BaseController
    {
        private readonly IShopService _shopService;

        public ShopController(
      IShopService shopService,
      UserManager<AppUser> userManager,
      RoleManager<AppRole> roleManager,
      SignInManager<AppUser> signInManager,
      IWebHostEnvironment webHostEnvironment,
      IMapper mapper)
      : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var result = await _shopService.GetShopBySellerIdAsync(user.Id);

            if (!result.Success || result.Data == null)
            {
                TempData["ErrorMessage"] = "No shop found. Please contact admin.";
                return View(null);
            }

            return View(result.Data); 
        }

    }
}
