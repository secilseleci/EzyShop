using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Order;

namespace WebUI.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        private readonly IShoppingCartItemService _shoppingCartItemService;
        public OrderController(IShoppingCartItemService shoppingCartItemService,
            IOrderService orderService,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
            _shoppingCartItemService = shoppingCartItemService;
            _orderService = orderService;
        }

        #region Summary and Place Order 
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var userId = GetUserId();
            var result = await _orderService.GetOrderSummaryAsync(userId);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index", "ShoppingCart");
            }

            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(SummaryViewModel model)
        {
            var result = await _orderService.CreateOrderAsync(model);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Summary", "ShoppingCart");
            }

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction("OrderSuccess", "Order", new { orderCodes = result.Data });
        }
        public IActionResult OrderSuccess(List<string> orderCodes)
        {
            ViewBag.OrderCodes = orderCodes;  
            return View();
        }
        #endregion



        #region List Order
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null || user.Shop == null)
            {
                TempData["ErrorMessage"] = "You don't have a shop!";
                return RedirectToAction("Index", "Home");
            }

            var result = await _orderService.GetAllOrdersAsync(user.Shop.Id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<OrderViewModel>());
            }

            return View(result.Data);
        }



        #endregion






    }
}
