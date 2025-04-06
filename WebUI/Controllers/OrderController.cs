//using AutoMapper;
//using Business.Services.Abstract;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Models.Identity;
//using Models.ViewModels.Order;
//using System.Drawing.Drawing2D;

//namespace WebUI.Controllers
//{
//    [Authorize]
//    public class OrderController : BaseController
//    {
//        private readonly IOrderService _orderService;
//        private readonly IShopService _shopService;

//        private readonly IShoppingCartItemService _shoppingCartItemService;
//        public OrderController(IShoppingCartItemService shoppingCartItemService,
//            IOrderService orderService,
//            IShopService shopService,
//            UserManager<AppUser> userManager,
//            RoleManager<AppRole> roleManager,
//            SignInManager<AppUser> signInManager,
//            IWebHostEnvironment webHostEnvironment,
//            IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
//        {
//            _shoppingCartItemService = shoppingCartItemService;
//            _orderService = orderService;
//            _shopService = shopService;
//        }

//        #region Summary and Place Order 
//        [HttpGet]
//        public async Task<IActionResult> Summary()
//        {
//            var userId = GetUserId();
//            var result = await _orderService.GetOrderSummaryAsync(userId);

//            if (!result.Success)
//            {
//                TempData["ErrorMessage"] = result.Message;
//                return RedirectToAction("Index", "ShoppingCart");
//            }

//            return View(result.Data);
//        }
//        [HttpPost]
//        public async Task<IActionResult> PlaceOrder(SummaryViewModel model)
//        {
//            var result = await _orderService.CreateOrderAsync(model);

//            if (!result.Success)
//            {
//                TempData["ErrorMessage"] = result.Message;
//                return RedirectToAction("Summary", "ShoppingCart");
//            }

//            TempData["SuccessMessage"] = "Order placed successfully!";
//            return RedirectToAction("OrderSuccess", "Order", new { orderCodes = result.Data });
//        }
//        public IActionResult OrderSuccess(List<string> orderCodes)
//        {
//            ViewBag.OrderCodes = orderCodes;  
//            return View();
//        }
//        #endregion

//        #region List Order
 
//        [HttpGet]
//        public IActionResult Index()
//        {
//            return View();
//        }

//        #region API
//        [HttpGet]

//        public async Task<IActionResult> GetSellerOrders()
//        {
//            var user = await GetCurrentUserAsync();
//            if (user == null || user.Shop == null)
//            {
//                return Json(new { data = new List<object>() });
//            }

//            var orders = await _orderService.GetAllOrdersAsync(user.Shop.Id);
//            if (!orders.Success || orders.Data == null)
//            {
//                return Json(new { data = new List<OrderViewModel>(), error = orders.Message });
//            }
//            return Json(new { data = orders.Data });

//        }
//        #endregion


//        #endregion

//        #region Order Detail
//        [HttpGet]
//        public IActionResult OrderDetail(Guid id)
//        {
//            return View();
//        }
//        [HttpGet("Order/GetOrderDetail/{orderId}")]
//        public async Task<IActionResult> GetOrderDetail(Guid orderId)
//        {
//            var user = await GetCurrentUserAsync();
//            if (user == null)
//            {
//                return Unauthorized();
//            }
//            var orderResult = await _orderService.GetOrderByIdAsync(orderId);
//            if (!orderResult.Success || orderResult.Data == null)
//            {
//                return NotFound();
//            }

//            return Json(new { data = orderResult.Data });
//        }
//        #endregion

//        #region Mark As Shipped

//        [HttpPost]
//        public async Task<IActionResult> MarkAsShipped(Guid orderId)
//        {
//            var result = await _orderService.MarkAsShipped(orderId);
//            if (!result.Success)
//            {
//                return Json(new { success = false, message = result.Message });

//            }
//            return Json(new { success = true, message = "Order marked as shipped successfully!", redirectUrl = Url.Action("Index", "Order") });

//        }


//        #endregion
//    }
//}
