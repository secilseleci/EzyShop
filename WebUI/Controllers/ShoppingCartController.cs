using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Customer")]

    public class ShoppingCartController : BaseController
    {private readonly IShoppingCartService _shoppingCartService;
        private readonly IShoppingCartItemService _shoppingCartItemService;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IShoppingCartItemService shoppingCartItemService,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
            : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
            _shoppingCartService = shoppingCartService;
            _shoppingCartItemService = shoppingCartItemService;
        }
        
        #region list cartitems
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var result = await _shoppingCartItemService.GetAllCartItemsAsync(user.Id);

            if (!result.Success || result.Data == null || !result.Data.Any())
            {
                TempData["WarningMessage"] = "Sepetinizde ürün bulunmamaktadır.";
                return View(new List<ShoppingCartItemViewModel>()); // Boş liste döndürüyoruz
            }

            return View(result.Data);
        }
        #endregion


        #region add cartitems
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId, int count)
        {
            if (count < 1 || count > 100)
            {
                return Json(new { success = false, message = "Adet en az 1, en fazla 100 olabilir." });
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();
          
            var result = await _shoppingCartItemService.AddToCartAsync(user.Id, productId, count);

            if (!result.Success)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = "Product added to cart successfully!" });
        }
        #endregion
        #region update quantity
        
        #endregion
    }
}
