using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
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

            if (!result.Success || !result.Data.Any())
            {
                TempData["WarningMessage"] = Messages.ShoppingCartEmpty;
                return View(new List<ShoppingCartItemViewModel>());  
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
                return Json(new { success = false, message = Messages.CartItemCountError });
            }
           

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Json(new { success = false, redirect = "/Account/Login" });
            }


            var result = await _shoppingCartItemService.AddToCartAsync(user.Id, productId, count);

            if (!result.Success)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = Messages.AddShoppingCartItemSuccess });
        }
        #endregion

        #region remove cartitem
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Json(new { success = false, redirect = "/Account/Login" });
            }

            var cartItemResult = await _shoppingCartItemService.GetCartItemByIdAsync(itemId);
            if (!cartItemResult.Success || cartItemResult.Data == null)
            {
                return Json(new { success = false, message = Messages.ProductIsNotInYourCart });
            }

            var cartItem = cartItemResult.Data;
            var result = await _shoppingCartItemService.RemoveItemFromCartAsync(user.Id,cartItem.ProductId);
            return Json(new { success = result.Success, message = result.Message });

        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Json(new { success = false, redirect = "/Account/Login" });
            }

            var clearResult = await _shoppingCartItemService.ClearCartAsync(user.Id);
            return Json(new { success = clearResult.Success, message = clearResult.Message });
        }


        #endregion

        #region update quantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(Guid itemId, string action)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Json(new { success = false, redirect = "/Account/Login" });
            }

            if (string.IsNullOrEmpty(action))
            {
                return Json(new { success = false, message = "Invalid transaction" });
            }

            var cartItemResult = await _shoppingCartItemService.GetCartItemByIdAsync(itemId);
            if (!cartItemResult.Success || cartItemResult.Data == null)
            {
                return Json(new { success = false, message = Messages.ProductIsNotInYourCart });
            }

            var cartItem = cartItemResult.Data;

            Core.Utilities.Results.IResult result;
            if (action.ToLower() == "increase")
            {
                result = await _shoppingCartItemService.IncreaseItemCountAsync(user.Id, cartItem.ProductId);
            }
            else if (action.ToLower() == "decrease")
            {
                result = await _shoppingCartItemService.DecreaseItemCountAsync(user.Id, cartItem.ProductId);
            }
            else
            {
                return Json(new { success = false, message = "Invalid transaction" });
            }

            return Json(new { success = result.Success, message = result.Message });
        }
        #endregion
        
        #region show total count
        [HttpGet]
        public async Task<IActionResult> GetCartItemCount()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Json(new { success = false, count = 0 });
            }
            int totalItems = await _shoppingCartItemService.GetTotalCartItemsAsync(user.Id);
            return Json(new { success = true, count = totalItems });
        }
        #endregion

       
    }
}
