using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Seller.Controllers;

public class BaseSellerController : BaseController
{
    protected readonly ISellerService _sellerService;
    private readonly IShopService _shopService;

    public BaseSellerController(
        ISellerService sellerService,
        IShopService shopService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
        : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _sellerService = sellerService;
        _shopService = shopService;
    }

    protected async Task<Guid?>GetCurrentShopIdAsync()
    {
        if (!CurrentUserId.HasValue)
            return null;

        var sellerResult = await _sellerService.GetActiveSellerByUserIdAsync(CurrentUserId.Value);
        if (!sellerResult.Success || sellerResult.Data == null)
            return null;

        var shopResult = await _shopService.GetActiveShopBySellerIdAsync(sellerResult.Data.Id);
        if (!shopResult.Success || shopResult.Data == null)
            return null;

        return shopResult.Data.Id;
    }
}
