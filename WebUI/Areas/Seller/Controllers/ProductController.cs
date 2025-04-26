using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Product;

namespace WebUI.Areas.Seller.Controllers;

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
        return RedirectToAction("Available");
    }

    #endregion

    #region List
    [HttpGet]
    public IActionResult Available()
    {
        ViewBag.Status = "Available";
        return View("ProductList");
    }

    [HttpGet]
    public IActionResult SoldOut()
    {
        ViewBag.Status = "SoldOut";
        return View("ProductList");
    }

    #endregion 

    #region API
    [HttpGet]
    public async Task<IActionResult> GetProducts(string status)
    {
        int.TryParse(Request.Query["start"], out int start);
        int.TryParse(Request.Query["length"], out int length);
        string? search = Request.Query["search[value]"];
        int page = (start / length) + 1;
        int pageSize = length;


        if (!Enum.TryParse<ProductStatus>(status, true, out var productStatus))
            return Json(new { success = false, message = "Invalid status parameter" });

        var currentShopId = await GetCurrentShopIdAsync();
        if (!currentShopId.HasValue)
        {
            return Json(new { success = false, message = Messages.NotAllowed });
        }


        var result = await _productService.GetProductsAsync(productStatus, currentShopId.Value, search, page, pageSize);

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

    #region Product Detail
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, string status)
    {
        var currentShopId = await GetCurrentShopIdAsync();

        if (currentShopId==null)
        {
            return Unauthorized();
        }
 
        var result = await _productService.GetProductDetailsAsync(currentShopId.Value, id);

        if (!result.Success)
            return NotFound();

        return PartialView("_ProductDetailsPartial", result.Data);
    }
    #endregion
}
