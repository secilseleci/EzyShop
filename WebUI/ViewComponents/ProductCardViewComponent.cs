using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.ViewComponents
{
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
            var productsResult = await _productService.GetFilteredProductsAsync(name, category, color, minPrice, maxPrice);

            if (!productsResult.Success || productsResult.Data == null)
            {
                return View(new List<ProductViewModel>());
            }

            var productViewModels = _mapper.Map<IEnumerable<ProductViewModel>>(productsResult.Data);

            return View(productViewModels);
        }
    }
}