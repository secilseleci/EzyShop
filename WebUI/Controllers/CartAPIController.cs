using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.OrderItem;

namespace WebUI.Controllers;
[Route("api/cart")]
[ApiController]
[Authorize(Roles = "Customer")]

public class CartAPIController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IOrderItemService _orderItemService;
    public CartAPIController(
     IOrderService orderService,
     IOrderItemService orderItemService)
    {
        _orderService = orderService;
        _orderItemService = orderItemService;
    }

    [HttpPost("addtocart")]
    public async Task<IActionResult> AddToCart([FromForm] Guid productId)
    {
        var result = await _orderService.AddToCartAsync(productId);
        return ApiResult(result);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCartItemCount()
    {
        var result = await _orderService.GetInCartOrderAsync();
        if (!result.Success || result.Data == null)
            return Ok(new { count = 0 });

        var count = result.Data.OrderItems.Where(x => !x.IsDeleted).Sum(x => x.Count);
        return Ok(new { count });
    }

    [HttpGet("getcart")]
    public async Task<IActionResult> GetCart()
    {
        var result = await _orderService.GetCartPageAsync();
        return ApiResult(result);
    }

    [HttpPost("updatecount")]
    public async Task<IActionResult> UpdateCount([FromBody] UpdateOrderItemCountDto dto)
    {
        var result = await _orderItemService.UpdateOrderItemCountAsync(dto.OrderItemId, dto.Delta);
        return ApiResult(result);
    }
    [HttpPost("removeitem")]
    public async Task<IActionResult> RemoveItem([FromBody] Guid orderItemId)
    {
        var result = await _orderItemService.DeleteOrderItemAsync(orderItemId);
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        bool isEmpty = await _orderService.IsCartEmptyAsync();

        return Ok(new { success = true, message = result.Message, isCartEmpty = isEmpty });
    }
}
