using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Product;

namespace WebUI.Controllers;
[ApiController]
[Route("api/products")]

public class ProductAPIController : BaseApiController
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public ProductAPIController(
     IProductService productService,
     ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }
     
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();

        return ApiResult(result);
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

}
