using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers;

public class ProductController: BaseController
{
    private readonly IProductService _productService;
     public ProductController(
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper,
     IProductService productService)
    : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _productService = productService;
    }

 
    public IActionResult Index()
    {
        return View();
    }
   
    [HttpGet]
    public async Task<IActionResult> Details(Guid productId)
    { 
        var result = await _productService.GetProductDetailsForCustomerAsync(productId);
        
        if (!result.Success || result.Data == null)
            return RedirectToAction("Login", "Auth", new { error = result.Message });

        return View(result.Data);
    }

  
}
