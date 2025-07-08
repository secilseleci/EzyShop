using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Product;
using WebUI.Controllers;

namespace WebUI.Areas.Seller.Controllers;

[Area("Seller")]
[Authorize(Roles = "Seller")]
public class ProductController : BaseController
{
    private readonly IProductService _productService;
    public ProductController(
        IProductService productService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
        : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _productService = productService;
    }


    #region List Products
    [HttpGet]
    public IActionResult Available()
    {
        ViewBag.Status = "Available";
        return View("ProductList");
    }

    [HttpGet]
    public ActionResult SoldOut()
    {
        ViewBag.Status = "SoldOut";
        return View("ProductList");
    }
    #endregion

    #region Product Detail
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, string status)
    {
        var result = await _productService.GetProductDetailsForSellerAsync( id);

        if (!result.Success)
            return NotFound();
        ViewBag.Status = status;

        return PartialView("_ProductDetailsPartial", result.Data);
    }
    #endregion

    #region Create Product
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateProductViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductViewModel model, IFormFile? file)
    {
        if (!ModelState.IsValid)
            return View(model);

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

    #region Update Product
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("Available");
        }

        var model = Mapper.Map<UpdateProductViewModel>(result.Data);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductViewModel model, IFormFile? file)
    {
        if (!ModelState.IsValid)
            return View(model);

        HandleImageUpload(model, file);

        var result = await _productService.UpdateProductAsync(model);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Available");
    }
    #endregion
}