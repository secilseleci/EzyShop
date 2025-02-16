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
            var categoriesResult = await _categoryService.GetAllCategoriesAsync(c=>true);
            var model = new ProductFilterViewModel
            {
                Categories = categoriesResult.Data != null
            ? categoriesResult.Data.Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name }).ToList()
            : new List<CategoryViewModel>()
            };
            return View(model);
        }
    }
}
