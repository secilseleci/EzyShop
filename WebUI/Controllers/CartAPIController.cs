using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;
[Route("api/cart")]
[ApiController]
public class CartAPIController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IOrderItemService _orderItemService;
    public CartAPIController(
     IOrderService orderService,
     IOrderItemService orderItemService,
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _orderService = orderService;
        _orderItemService = orderItemService;
    }

    [HttpPost("addtocart")]
    public async Task<IActionResult> AddToCart([FromForm] Guid productId)
    {
        if (!CurrentUserId.HasValue)
            return RedirectToAction("Login", "Auth", new
            {
                error = Messages.LoginUnauthorized
            });

        var result = await _orderService.GetOrCreateCartAndAddProductAsync(productId, CurrentUserId.Value);
        if (!result.Success)
             return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message });
    }
}
