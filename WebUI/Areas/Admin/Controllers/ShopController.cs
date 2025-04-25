using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ShopController : BaseController
{
    private readonly IShopService _shopService;
    public ShopController(
        UserManager<AppUser> userManager,
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

    #region List Shops
    public IActionResult Pending()
    {
        ViewBag.Status = "Pending";
        return View("ShopList");
    }
    public IActionResult Active()
    {
        ViewBag.Status = "Active";
        return View("ShopList");
    }
    public IActionResult Inactive()
    {
        ViewBag.Status = "Inactive";
        return View("ShopList");
    }
    public IActionResult Deleted()
    {
        ViewBag.Status = "Deleted";
        return View("ShopList");
    }
    #region API
    [HttpGet]
    public async Task<IActionResult> GetShops(string status)
    {
        int.TryParse(Request.Query["start"], out int start);
        int.TryParse(Request.Query["length"], out int length);
        string? search = Request.Query["search[value]"];
        int page = (start / length) + 1;
        int pageSize = length;

        if (!Enum.TryParse<ShopStatus>(status, true, out var shopStatus))
            return Json(new { success = false, message = "Invalid status parameter" });

        var result = await _shopService.GetShopsAsync(shopStatus, search, page, length);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new
        {
            draw = int.Parse(Request.Query["draw"]),
            recordsTotal = result.Data.TotalItems,
            recordsFiltered = result.Data.TotalItems,
            data = result.Data.Items
        });
    }

    #endregion


    #endregion

    #region Shop Detail
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, string status)
    {
        var result = await _shopService.GetShopDetailsAsync(id);

        if (!result.Success)
            return NotFound();

        ViewBag.Status = status;

        return PartialView("_ShopDetailsPartial", result.Data);
    }
    #endregion

    #region Approve
    [HttpPost]
    public async Task<IActionResult> ApproveShop(Guid shopId, Guid sellerId)
    {
        var result = await _shopService.ApproveShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }
        return Json(new { success = true, message = result.Message });
    }
    #endregion

    #region Reject
    [HttpPost]
    public async Task<IActionResult> RejectShop(Guid shopId, Guid sellerId)
    {
        var result = await _shopService.RejectShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }
        return Json(new { success = true, message = result.Message });
    }
    #endregion

    #region Deactivate
    [HttpPost]
    public async Task<IActionResult> DeactivateShop(Guid shopId, Guid sellerId)
    {
        var result = await _shopService.DeactivateShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }
        return Json(new { success = true, message = result.Message });
    }
    #endregion

    #region Reactivate
    [HttpPost]
    public async Task<IActionResult> ReactivateShop(Guid shopId, Guid sellerId)
    {
        var result = await _shopService.ReactivateShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }
        return Json(new { success = true, message = result.Message });
    }
    #endregion

    #region Delete
    [HttpPost]
    public async Task<IActionResult> DeleteShop(Guid shopId, Guid sellerId)
    {
        var result = await _shopService.DeleteShopAsync(shopId, sellerId);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }
        return Json(new { success = true, message = result.Message });
    }
    #endregion
}
