using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.OrderItem;
using Models.Identity;

namespace WebUI.Controllers;
[Route("api/cart")]
[ApiController]
[Authorize(Roles = "Customer")]

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


    [HttpGet("getcart")]
    public async Task<IActionResult>GetCart()
    {
        var result = await _orderService.GetCartPageAsync();
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message, data=result.Data });
    }


    [HttpPost("addtocart")]
    public async Task<IActionResult> AddToCart([FromForm] Guid productId)
    {
        var result = await _orderService.AddToCartAsync(productId);
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message });
    }

    [HttpPost("updatecount")]
    public async Task<IActionResult> UpdateCount([FromBody] UpdateOrderItemCountDto dto)
    {
        var result = await _orderItemService.UpdateOrderItemCountAsync(dto.OrderItemId, dto.Delta);
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message });
    }

}
