using AutoMapper;
using Core.Constants;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;

public class ValidationController:BaseController
{
    private readonly IShopRepository _shopRepo;
    private readonly ISellerRepository _sellerRepo;

    public ValidationController(
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper,
     IShopRepository shopRepo,
     ISellerRepository sellerRepo) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _shopRepo = shopRepo;
        _sellerRepo = sellerRepo;
    }


    #region Check Email
    [HttpGet]
    public async Task<JsonResult> CheckEmailAvailability(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);
        bool available = user == null;

        return Json(new
        {
            available,
            message = available ? "" : Messages.AlreadyRegistered
        });
    }
    #endregion


    #region Check ShopName
    [HttpGet]
    public async Task<JsonResult> CheckShopNameAvailability(string shopName)
    {
        var exists = await _shopRepo.ExistsAsync(s => s.Name.ToLower().Trim() == shopName.ToLower().Trim() && !s.IsDeleted);
        return Json(new
        {
            available = !exists,
            message = exists ? Messages.AlreadyExistsShopName : ""
        });
    }
    #endregion

    #region Check TaxNumber
    [HttpGet]
    public async Task<JsonResult> CheckTaxNumberAvailability(string taxNumber)
    {
        var exists = await _shopRepo.ExistsAsync(s => s.TaxNumber.Trim() == taxNumber.Trim() && !s.IsDeleted);
        return Json(new
        {
            available = !exists,
            message = exists ? Messages.AlreadyExistsTaxNumber : ""
        });
    }
    #endregion

    #region Check PhoneNumber
    [HttpGet]
    public async Task<JsonResult> CheckPhoneNumberAvailability(string phoneNumber)
    {
        var exists = await _sellerRepo.ExistsAsync(s => s.Phone.Trim() == phoneNumber.Trim() && !s.IsDeleted);
        return Json(new
        {
            available = !exists,
            message = exists ? Messages.AlreadyExistsPhone : ""
        });
    }
    #endregion
}
