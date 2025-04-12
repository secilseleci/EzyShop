using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;


namespace WebUI.Controllers;

[Authorize(Roles = "Customer")]

public class CartController : BaseController
{
    private readonly ICartService _cartService;
    private readonly ICartLineService _cartLineService;

    public CartController(
        ICurrentUserService currentUserService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper,
        ICartService cartService,
        ICartLineService cartLineService

        )
        : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _cartService = cartService;
        _cartLineService = cartLineService;
    }

    #region cartitem count
    [HttpGet]
    public async Task<IActionResult> GetCartLineCount()
    {
        if (!CurrentUserService.UserId.HasValue)
            return Json(0);
        
        int totalItems = await _cartLineService.GetTotalCartLinesAsync(CurrentUserService.UserId.Value);
        return Json(new { success = true, count = totalItems });
    }
    #endregion

    #region add to cart
    [HttpPost]
    public async Task<IActionResult> AddToCart(Guid productId)
    {
        if (!CurrentUserService.UserId.HasValue)
            return Json(new { success = false, message = Messages.UserNotAuthenticated });

        var result = await _cartLineService.CreateCartLineAsync(CurrentUserService.UserId.Value, productId);

        return Json(new { success = result.Success, message = result.Message });
    }


    #endregion
}