using AutoMapper;
using Business.Services.Abstract;
using Business.Services.Concrete;
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

    }
}
