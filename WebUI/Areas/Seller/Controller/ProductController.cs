using AutoMapper;
using Business.Services.Abstract;
using Business.Services.Concrete;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Product;
 
namespace WebUI.Areas.Seller.Controller;

[Area("Seller")]
[Authorize(Roles = "Seller")]
public class ProductController : BaseSellerController
{
    private readonly IProductService _productService;

    public ProductController(
        IProductService productService,
        ISellerService sellerService,
        IShopService shopService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
        : base(sellerService, shopService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _productService = productService;
    }
    #region Create Product
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var currentShopId = await GetCurrentShopIdAsync();
        if (!currentShopId.HasValue)
        {
            TempData["ErrorMessage"] = Messages.NotAllowed;
            return RedirectToAction("Index");
        }

        return View(new CreateProductViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductViewModel model, IFormFile? file)
    {  
        if (!ModelState.IsValid)
            return View(model);

        var currentShopId = await GetCurrentShopIdAsync();
        if (!currentShopId.HasValue)
        {
            TempData["ErrorMessage"] = Messages.NotAllowed;
            return RedirectToAction("Index");
        }

        model.ShopId = currentShopId.Value;
        HandleImageUpload(model, file);

        var result = await _productService.CreateProductAsync(model);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Index");
    }

    #endregion

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    #region API
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        int page = 1;
        int pageSize = 10;
        int draw = 0;

        // Query string'den oku ve TryParse ile güvenli yakala
        if (int.TryParse(Request.Query["start"], out int start) &&
            int.TryParse(Request.Query["length"], out int length) &&
            length > 0)
        {
            page = (start / length) + 1;
            pageSize = length;
        }

        int.TryParse(Request.Query["draw"], out draw);
        string? search = Request.Query["search[value]"];
        
        var currentShopId = await GetCurrentShopIdAsync();
        if (!currentShopId.HasValue)
        {
            return Json(new { success = false, message = Messages.NotAllowed });
        }
       
        var result = await _productService.GetProductsAsync(currentShopId.Value, search, page, pageSize);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new
        {
            draw = draw,
            recordsTotal = result.Data.TotalItems,
            recordsFiltered = result.Data.TotalItems,
            data = result.Data.Items
        });
    }

    #endregion
}
