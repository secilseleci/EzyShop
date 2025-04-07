using Core.Pagination;
using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.Category;

namespace Business.Services.Abstract;

public interface ICategoryService
{
    Task<IDataResult<Category>> GetCategoryByIdAsync(Guid categoryId);
    Task<IDataResult<IEnumerable<CategoryViewModel>>> GetAllCategoriesAsync();
    Task<IDataResult<PaginatedList<CategoryViewModel>>> GetPaginatedCategoriesAsync(int page,
        int pageSize,
        string? searchTerm = null);

    Task<IResult> CreateCategoryAsync(CategoryViewModel model);
    Task<IResult> UpdateCategoryAsync(CategoryViewModel model);
    Task<IResult> DeleteCategoryAsync(Guid categoryId );
 
}
