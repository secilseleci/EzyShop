using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Product;
using WebUI.Controllers;

namespace WebUI.Areas.Seller.Controllers;

[Area("Seller")]
[Route("Seller/[controller]/[action]")]
[Authorize(Roles = "Seller")]
public class ProductController : BaseController
{
    private readonly IProductService _productService;
    private readonly IShopService _shopService;
    private readonly ISellerService _sellerService;

    public ProductController
    (
        ICurrentUserService currentUserService,
        ISellerService sellerService,
        IProductService productService,
        IShopService shopService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
        : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _sellerService = sellerService;
        _productService = productService;
        _shopService = shopService;
    }

    #region List Products

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    #endregion

    #region API
    [HttpGet]
    public async Task<IActionResult> GetPaginatedProducts()
    {
        if (!CurrentUserService.UserId.HasValue)
            return Unauthorized();

        int start = int.Parse(Request.Query["start"]);
        int length = int.Parse(Request.Query["length"]);
        string? search = Request.Query["search[value]"];
        int page = (start / length) + 1;
        int pageSize = length;


        var result = await _productService.GetPaginatedProductsForSellerAsync(CurrentUserService.UserId.Value, page, length, search);

        if (!result.Success)
        {
            return Json(new
            {
                success = false,
                message = result.Message
            });
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

    #region Delete Product
    [HttpPost]
    public async Task<IActionResult> Delete(Guid productId)
    {
        var productResult = await _productService.GetProductByIdAsync(productId);
        if (!productResult.Success)
        {
            return Json(productResult);
        }

        var deleteResult = await _productService.DeleteProductAsync(productId);
        if (!deleteResult.Success)
        {
            TempData["ErrorMessage"] = deleteResult.Message;
            return Json(deleteResult);
        }

        DeleteOldImage(productResult.Data.ImageUrl, WebHostEnvironment.WebRootPath);
        TempData["SuccessMessage"] = deleteResult.Message;
        return Json(deleteResult);
    }

    #endregion

    #region Create Product
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (!CurrentUserService.UserId.HasValue)
            return Unauthorized();

        var checkResult = await _shopService.CheckShopIsActiveAsync(CurrentUserService.UserId.Value);
        if (!checkResult.Success)
        {
            TempData["ErrorMessage"] = checkResult.Message;
            return RedirectToAction("Index", "Dashboard");
        }

        return View(new ProductCreateViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel model, IFormFile? file)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!CurrentUserService.UserId.HasValue)
            return Unauthorized();


        HandleImageUpload(model, file);

        var result = await _productService.CreateProductAsync(model, CurrentUserService.UserId.Value);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Index");
    }

    #endregion

    #region Edit
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!CurrentUserService.UserId.HasValue)
            return Unauthorized();

        var checkResult = await _shopService.CheckShopIsActiveAsync(CurrentUserService.UserId.Value);
        if (!checkResult.Success)
        {
            TempData["ErrorMessage"] = checkResult.Message;
            return RedirectToAction("Index", "Dashboard");
        }

        var result = await _productService.GetProductByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var model = Mapper.Map<ProductUpdateViewModel>(result.Data);
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(ProductUpdateViewModel model, IFormFile? file)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!CurrentUserService.UserId.HasValue)
            return Unauthorized();

        HandleImageUpload(model, file);

        var result = await _productService.UpdateProductAsync(model, CurrentUserService.UserId.Value);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Index");
    }



    #endregion

    #region Toggle
    [HttpPost]
    public async Task<IActionResult> ToggleStatus(Guid productId)
    {
        if (!CurrentUserService.UserId.HasValue)
            return BadRequest(new { success = false, message = "User not authenticated." });
        var userId = CurrentUserService.UserId.Value;

        var result = await _productService.ToggleProductStatusAsync(productId, userId);

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new
        {
            success = true,
            message = result.Message
        });
    }
    #endregion
}
