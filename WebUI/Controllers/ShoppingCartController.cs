using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers
{
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

        [Authorize(Roles = "Customer")]
        [HttpGet]

        #endregion
        #region add cartitems

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();

            var result = await _shoppingCartItemService.AddToCartAsync(user.Id, productId, quantity);

            if (!result.Success)
            {
                return Json(new { success = false, message = result.Message });
            }

            var cartItemCount = await _shoppingCartItemService.GetTotalCartItemsAsync(user.Id);
            return Json(new { success = true, message = "Product added to cart successfully!", cartItemCount });
        }
        #endregion
       
    }
}
