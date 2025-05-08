using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Cart;

namespace WebUI.Controllers;

public class CartController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IOrderItemService _orderItemService;
    private readonly ICustomerService _customerService;
   public CartController(
     ICustomerService customerService,
     IOrderService orderService,
     IOrderItemService orderItemService,
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _customerService = customerService;
        _orderService = orderService;
        _orderItemService = orderItemService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!CurrentUserId.HasValue)
            return RedirectToAction("Login", "Auth", new { error = Messages.LoginUnauthorized });

        var result = await _orderService.GetCartPageAsync();

        if (!result.Success || result.Data.OrderItems.Count == 0)
        {
            ViewBag.IsCartEmpty = true;
            return View();
        }

        return View(result.Data);

    }


    [HttpGet]
    public async Task<IActionResult> Confirm()
    {
        if (!CurrentUserId.HasValue)
            return RedirectToAction("Login", "Auth", new { error = Messages.LoginUnauthorized });

      
    }
}
