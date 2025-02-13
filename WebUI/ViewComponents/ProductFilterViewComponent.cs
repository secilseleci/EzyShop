using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.ViewComponents
{
    public class ProductFilterViewComponent:ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public ProductFilterViewComponent(IProductService productService, IMapper mapper, ICategoryService categoryService)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var model = new ProductFilterViewModel
            //{
            //    Categories = (await _categoryService.GetAllCategoriesAsync(p => true))

            //};

            return View();  
        }
    }
}
