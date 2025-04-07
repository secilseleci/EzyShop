using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Pagination;
using Core.Security;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.ViewModels.Category;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class CategoryService : BaseService, ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryService(
     ICategoryRepository categoryRepo,
     IMapper mapper,
     IConfiguration config,
     ICurrentUserService currentUser
    ) : base(mapper, config, currentUser)
    {
        _categoryRepo = categoryRepo;
    }

    #region Read

    public async Task<IDataResult<Category>> GetCategoryByIdAsync(Guid categoryId)
    {
        var category = await _categoryRepo.GetByIdAsync(categoryId);
        return category is not null
            ? new SuccessDataResult<Category>(category)
            : new ErrorDataResult<Category>(Messages.CategoryNotFound);
    }

    public async Task<IDataResult<IEnumerable<CategoryViewModel>>> GetAllCategoriesAsync()
    {
        var categoryList = await _categoryRepo.GetAllAsync();
        var viewModels = Mapper.Map<IEnumerable<CategoryViewModel>>(categoryList);

        return categoryList is not null && categoryList.Any()
            ? new SuccessDataResult<IEnumerable<CategoryViewModel>>(viewModels)
            : new ErrorDataResult<IEnumerable<CategoryViewModel>>(Messages.EmptyCategoryList);

    }

    public async Task<IDataResult<PaginatedList<CategoryViewModel>>> GetPaginatedCategoriesAsync(int page, int pageSize, string? searchTerm = null)
    {
        Expression<Func<Category, bool>> predicate;
        if (!string.IsNullOrWhiteSpace(searchTerm))

        {
            predicate = c =>
            (c.Name).Contains(searchTerm);
        }
        else
        {
            predicate = c => true;
        }

        var paginatedCategories = await _categoryRepo.GetPaginatedAsync(
            predicate,
            page,
            pageSize);

        var viewModels = Mapper.Map<IEnumerable<CategoryViewModel>>(paginatedCategories.Items);

        var result = new PaginatedList<CategoryViewModel>(
            viewModels,
            paginatedCategories.TotalItems,
            paginatedCategories.Page,
            paginatedCategories.PageSize
        );

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<CategoryViewModel>>(result)
            : new ErrorDataResult<PaginatedList<CategoryViewModel>>(Messages.EmptyCategoryList);
    }

    #endregion

    #region Create
    public async Task<IResult> CreateCategoryAsync(CategoryViewModel model)
    {
        var exists = await _categoryRepo.ExistsAsync(c => c.Name.ToLower() == model.Name.ToLower() && !c.IsDeleted);
        if (exists)
        {
            return new ErrorResult(Messages.CategoryAlreadyExists);
        }

        var category = Mapper.Map<Category>(model);

        var addResult = await _categoryRepo.CreateAsync(category);

        return addResult > 0
            ? new SuccessResult(Messages.CreateCategorySuccess)
            : new ErrorResult(Messages.CreateCategoryError);
    }

    #endregion

    #region Delete
    public async Task<IResult> DeleteCategoryAsync(Guid categoryId)
    {
        var exists = await _categoryRepo.ExistsAsync(c => c.Id == categoryId && !c.IsDeleted);
        if (!exists)
            return new ErrorResult(Messages.CategoryNotFound);

        var affectedRows = await _categoryRepo.SoftDeleteAsync(categoryId);

        return affectedRows > 0
        ? new SuccessResult(Messages.DeleteCategorySuccess)
        : new ErrorResult(Messages.DeleteCategoryError);
    }

    #endregion

    #region Update
    public async Task<IResult> UpdateCategoryAsync(CategoryViewModel model)
    {
        var category = await _categoryRepo.GetByIdAsync(model.Id);
        if (category is null)
            return new ErrorResult(Messages.CategoryNotFound);

        var nameExists = await _categoryRepo.ExistsAsync(c =>
            c.Id != model.Id &&
            c.Name.ToLower() == model.Name.ToLower() &&
            !c.IsDeleted);

        if (nameExists)
            return new ErrorResult(Messages.CategoryAlreadyExists);

        if (category.Name == model.Name &&
           category.ImageUrl == model.ImageUrl)
        {
            return new ErrorResult(Messages.NoChangesDetected);
        }
        category = Mapper.Map(model, category);

        var updateResult = await _categoryRepo.UpdateAsync(category);

        return updateResult > 0
            ? new SuccessResult(Messages.UpdateCategorySuccess)
            : new ErrorResult(Messages.UpdateCategoryError);
    }
    #endregion

  



 
     
     
}
