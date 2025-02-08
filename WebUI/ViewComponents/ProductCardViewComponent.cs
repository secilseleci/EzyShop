using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.ViewComponents
{
    public class ProductCardViewComponent:ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductCardViewComponent(IProductService productService,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productService.GetAllProductsWithCategoryAsync(p => true);
            var mappedProducts = _mapper.Map<IEnumerable<ProductViewModel>>(products.Data);
            return View(mappedProducts);
        }
    }
}
