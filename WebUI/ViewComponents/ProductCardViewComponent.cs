using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.Product;

namespace WebUI.ViewComponents;

public class ProductCardViewComponent : ViewComponent
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductCardViewComponent(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
    {
        var productsResult = await _productService.GetFilteredPaginatedProductsForCustomerAsync(name, category, color, minPrice, maxPrice,1,10);

        if (!productsResult.Success || productsResult.Data == null)
        {
            return View(new List<FilteredProductCustomerViewModel>());
        }

        var productViewModels = _mapper.Map<IEnumerable<FilteredProductCustomerViewModel>>(productsResult.Data);

        return View(productViewModels);
    }
}