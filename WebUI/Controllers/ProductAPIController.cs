using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Product;

namespace WebUI.Controllers;

[Route("api/products")]
[ApiController]
public class ProductAPIController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public ProductAPIController(
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper,
     IProductService productService,
     ICategoryService categoryService) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpPost("filter")]
    public async Task<IActionResult> FilterProducts([FromForm] ProductFilterViewModel model)
    {
        var result = await _productService.GetFilteredProductsAsync(model);

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new
        {
            success = true,
            data = result.Data.Items,
            totalItems = result.Data.TotalItems,
            currentPage = result.Data.Page,
            pageSize = result.Data.PageSize
        });
    }
    #region List All Categories For Product Filter
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();

        if (!result.Success)
            return Json(new { success = false, message = result.Message });

        return Ok(new { success = true, data = result.Data });
    }
    #endregion
}
