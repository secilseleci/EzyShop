using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IShoppingCartItemService _shoppingCartItemService;
        public OrderController(IShoppingCartItemService shoppingCartItemService,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
            _shoppingCartItemService = shoppingCartItemService;

        }
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var cartItemsResult = await _shoppingCartItemService.GetAllCartItemsAsync(user.Id);
            if (!cartItemsResult.Success || cartItemsResult.Data == null || !cartItemsResult.Data.Any())
            {
                TempData["ErrorMessage"] = cartItemsResult.Message ?? "Your cart is empty.";
                return RedirectToAction("Index", "ShoppingCart");
            }
            var cartItems = cartItemsResult.Data;


            var shopOrders = cartItems
             .GroupBy(item => new { item.ShopId, item.ShopName })
             .Select(group => new ShopOrderSummaryViewModel
             {
                 ShopId = group.Key.ShopId,
                 ShopName = group.Key.ShopName,
                 OrderItems = Mapper.Map<List<OrderItemSummaryViewModel>>(group.ToList())
             }).ToList();

            var summaryViewModel = new OrderSummaryViewModel
            {
                CustomerId = user.Id,
                CustomerName = user.Name,
                Address = user.Address,
                ShopOrders = shopOrders
            };

            return View(summaryViewModel);

        }
    }
}
