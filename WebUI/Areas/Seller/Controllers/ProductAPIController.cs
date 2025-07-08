using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Seller.Controllers;

[ApiController]
[Area("Seller")]
[Route("seller/api/products")]

[Authorize(Roles = "Seller")]
public class ProductAPIController : BaseController
{
    private readonly IProductService _productService;

    public ProductAPIController(
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

    #region GetProducts
    [HttpGet]
    public async Task<IActionResult> GetProducts(string status, int start, int length)
    {
        string? search = Request.Query["search[value]"];
        int page = (start / length) + 1;
        int pageSize = length;


        if (!Enum.TryParse<ProductStatus>(status, true, out var productStatus))
            return BadRequest(new { success = false, message = "Invalid status parameter" });

        var result = await _productService.GetProductsAsync(productStatus,search, page, pageSize);

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }

        return Ok(new
        {
            draw = int.Parse(Request.Query["draw"]),
            recordsTotal = result.Data.TotalItems,
            recordsFiltered = result.Data.TotalItems,
            data = result.Data.Items
        });
    }

    #endregion
     
    #region Deactivate Product
    [HttpPost("deactivate")]
    public async Task<IActionResult> Deactivate([FromForm] Guid productId)
    {
        var result = await _productService.DeactivateProductAsync(productId);
        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }
        return Ok(new { success = true, message = result.Message });
    }
    #endregion

    #region Reactivate Product
    [HttpPost("reactivate")]
    public async Task<IActionResult> Reactivate([FromForm] Guid productId, [FromForm] int stock)
    {
        var result = await _productService.ReactivateProductAsync(productId, stock);
        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }
        return Ok(new { success = true, message = result.Message });
    }
    #endregion

    #region Delete Product
    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromForm] Guid productId)
    {
        var result = await _productService.DeleteProductAsync(productId);
        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }
        return Ok(new { success = true, message = result.Message });
    }
    #endregion
}



