using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;
[Route("[controller]/[action]")]

public class ProductController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    public ProductController(
        ICurrentUserService currentUserService,
        IProductService productService,
        ICategoryService categoryService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
         : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _productService = productService;
        _categoryService = categoryService;
    }
    #region List
    [HttpGet]
    public IActionResult Index()
    { return View(); }

    [HttpGet]
    public async Task<IActionResult> GetFilteredProducts(
     string? name, string? category, string? color,
     decimal? minPrice, decimal? maxPrice, int page = 1)
    {
        int pageSize = 9;

        var result = await _productService.GetFilteredProductsAsync(page, pageSize, name, category, color, minPrice, maxPrice);

        if (!result.Success)
            return Json(new { success = false, message = result.Message });

        return Json(new
        {
            success = true,
            data = result.Data.Items,
            totalItems = result.Data.TotalItems,
            currentPage = result.Data.Page,
            totalPages = result.Data.TotalPages
        });
    }

    #endregion

    #region List All Categories For Product Filter
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();

        if (!result.Success)
            return Json(new { success = false, message = result.Message });

        return Json(new { success = true, data = result.Data });
    }

    #endregion

    #region Details
    [HttpGet]
    public async Task<IActionResult> Details(Guid productId)
    {
        var result = await _productService.GetProductWithDetailsByIdAsync(productId);

        if (!result.Success || result.Data == null)
            return RedirectToAction("Index", new { error = result.Message });

        return View(result.Data);  
    }

    #endregion

}

