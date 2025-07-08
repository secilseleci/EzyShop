using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;
[Authorize(Roles = "Customer")]

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
        var result = await _orderService.GetCartPageAsync();

        if (!result.Success || result.Data == null || result.Data.OrderItems == null || !result.Data.OrderItems.Any())
        {
            return View("Empty");
        }

        return View(result.Data);
    }

    public IActionResult CartIcon()
    {
        return ViewComponent("CartIcon");
    }

    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        var order = await _orderService.GetInCartOrderAsync();
        if (!order.Success || order.Data == null)
            return RedirectToAction("Index");  

        var deleteResult= await _orderItemService.DeleteItemsAsync(order.Data.Id);
        if (!deleteResult.Success)
        {
            TempData["Error"] = deleteResult.Message;
            return RedirectToAction("Index");
        }
        return View("Empty");   
    }

    //[HttpGet]
    //public async Task<IActionResult> Confirm()
    //{
    //    if (!CurrentUserId.HasValue)
    //        return RedirectToAction("Login", "Auth", new { error = Messages.LoginUnauthorized });

    //    return View();
    //}
}
