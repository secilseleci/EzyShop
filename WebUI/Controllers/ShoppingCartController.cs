using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;


namespace WebUI.Controllers;

[Authorize(Roles = "Customer")]

public class ShoppingCartController : BaseController
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IShoppingCartItemService _shoppingCartItemService;

    public ShoppingCartController(
        ICurrentUserService currentUserService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper,
        IShoppingCartService shoppingCartService,
        IShoppingCartItemService shoppingCartItemService

        )
        : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _shoppingCartService = shoppingCartService;
        _shoppingCartItemService = shoppingCartItemService;
    }

    #region cartitem count
    [HttpGet]
    public async Task<IActionResult> GetCartItemCount()
    {
        if (!CurrentUserService.UserId.HasValue)
            return Json(0);
        
        int totalItems = await _shoppingCartItemService.GetTotalCartItemsAsync(CurrentUserService.UserId.Value);
        return Json(new { success = true, count = totalItems });
    }
    #endregion


}