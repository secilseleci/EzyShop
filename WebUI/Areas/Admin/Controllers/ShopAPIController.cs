using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;
[ApiController]
[Area("Admin")]
[Route("admin/api/shops")]
[Authorize(Roles = "Admin")]
public class ShopAPIController : BaseController
{
    private readonly IShopService _shopService;

    public ShopAPIController(UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IEmailService emailService,
    IMapper mapper,
    IShopService shopService)
    : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _shopService = shopService;
    }
    #region API
    [HttpGet]
    public async Task<IActionResult> GetShops(string status, int start, int length)
    {
        string? search = Request.Query["search[value]"];

        if (!Enum.TryParse<ShopStatus>(status, true, out var shopStatus))
            return BadRequest(new { success = false, message = "Invalid status parameter" });

        int page = (start / length) + 1;
        int pageSize = length;

        var result = await _shopService.GetShopsAsync(shopStatus, search, page, pageSize);

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }

        return Ok(new
        {
            draw = int.Parse(Request.Query["draw"]),
            recordsTotal = result.Data.TotalItems,
            recordsFiltered = result.Data.TotalItems,
            data = result.Data.Items
        });
    }

    #endregion

    #region Approve
    [HttpPost("approve")]
    public async Task<IActionResult> ApproveShop([FromForm] Guid shopId, [FromForm] Guid sellerId)
    {
        var result = await _shopService.ApproveShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }
        return Ok(new { success = true, message = result.Message });
    }
    #endregion

    #region Reject
    [HttpPost("reject")]
    public async Task<IActionResult> RejectShop([FromForm] Guid shopId, [FromForm] Guid sellerId)
    {
        var result = await _shopService.RejectShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }
        return Ok(new { success = true, message = result.Message });
    }
    #endregion

    #region Deactivate
    [HttpPost("deactivate")]
    public async Task<IActionResult> DeactivateShop([FromForm] Guid shopId, [FromForm] Guid sellerId)
    {
        var result = await _shopService.DeactivateShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }
        return Ok(new { success = true, message = result.Message });
    }
    #endregion

    #region Reactivate
    [HttpPost("reactivate")]
    public async Task<IActionResult> ReactivateShop([FromForm] Guid shopId, [FromForm] Guid sellerId)
    {
        var result = await _shopService.ReactivateShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }
        return Ok(new { success = true, message = result.Message });
    }
    #endregion

    #region Delete
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteShop([FromForm] Guid shopId, [FromForm] Guid sellerId)
    {
        var result = await _shopService.DeleteShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }
        return Ok(new { success = true, message = result.Message });
    }
    #endregion
}
