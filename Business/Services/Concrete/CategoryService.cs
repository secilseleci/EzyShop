using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        #region Read

        public async Task<IDataResult<Category>> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            return category is not null
                ? new SuccessDataResult<Category>(category)
                : new ErrorDataResult<Category>(Messages.CategoryNotFound);
        }

        public async Task<IDataResult<IEnumerable<Category>>> GetAllCategoriesAsync(
            Expression<Func<Category, bool>> predicate)
        {
            var categoryList = await _categoryRepository.GetAllAsync(predicate);
            var sortedList = categoryList?.OrderBy(c => c.DisplayOrder).ToList();

            return sortedList is not null && sortedList.Any()
                ? new SuccessDataResult<IEnumerable<Category>>(sortedList)
                : new ErrorDataResult<IEnumerable<Category>>(Messages.EmptyCategoryList);  
             
        }
        #endregion

        #region Create
        public async Task<IResult> CreateCategoryAsync(CategoryViewModel model)
        {
            var existResult = await CheckIfCategoryNameAlreadyExistsAsync(model);
            if (!existResult.Success)
            {
                return existResult;
            }

            int newDisplayOrder;

            if (model.DisplayOrder.HasValue) // Admin bir değer girdi mi?
            {
                newDisplayOrder = model.DisplayOrder.Value;

                // Eğer DisplayOrder belirtilmişse, sıralamayı kaydır
                var affectedCategories = await _categoryRepository
                    .GetAllAsync(c => c.DisplayOrder >= newDisplayOrder);

                if (affectedCategories != null && affectedCategories.Any())
                {
                    foreach (var category in affectedCategories.OrderByDescending(c => c.DisplayOrder))
                    {
                        category.DisplayOrder += 1;
                        await _categoryRepository.UpdateAsync(category);
                    }
                }
            }
            else
            {
                // Eğer DisplayOrder boş bırakıldıysa, en büyük DisplayOrder'ı bul ve +1 ekle
                newDisplayOrder = await _categoryRepository.GetMaxDisplayOrderAsync() + 1;
            }

            // Yeni kategoriyi ekleyelim
            var newCategory = _mapper.Map<Category>(model);
            newCategory.DisplayOrder = newDisplayOrder; // Belirlenen sıralamayı kullan

            var addResult = await _categoryRepository.CreateAsync(newCategory);

            return addResult > 0
                ? new SuccessResult(Messages.CreateCategorySuccess)
                : new ErrorResult(Messages.CreateCategoryError);
        }



        #endregion

        #region Update
        public async Task<IResult> UpdateCategoryAsync(CategoryViewModel model)
        {
            var categoryResult = await GetCategoryByIdAsync(model.Id);
            if (!categoryResult.Success)
            {
                return categoryResult;
            }

            var existResult = await CheckIfCategoryNameAlreadyExistsAsync(model);
            if (!existResult.Success)
            {
                return existResult;
            }

            var category = categoryResult.Data;
            int oldOrder = category.DisplayOrder;
            int newOrder = model.DisplayOrder ?? oldOrder; // boş bırakılmışsa, mevcut sırayı koru

            // hiçbir şey değişmemişse, güncellemeyi iptal et
            if (category.Name == model.Name &&
                category.ImageUrl == model.ImageUrl &&
                oldOrder == newOrder)
            {
                return new SuccessResult("Herhangi bir değişiklik yapmadınız");
            }

            // `DisplayOrder` değişmişse sıralamayı kaydır
            if (newOrder != oldOrder)
            {
                await ShiftCategoriesAfterUpdate(oldOrder, newOrder);
                category.DisplayOrder = newOrder;
            }

            // Sadece değişen değerleri güncelle
            CompleteUpdate(model, categoryResult);

            return await GetUpdateResultAsync(categoryResult);
        }
        #endregion

        #region Delete
        public async Task<IResult> DeleteCategoryAsync(Guid categoryId)
        {
            var categoryResult = await GetCategoryByIdAsync(categoryId);
            if (!categoryResult.Success)
            {
                return new ErrorResult(Messages.CategoryNotFound);
            }

            var deletedCategory = categoryResult.Data;
            int deletedOrder = deletedCategory.DisplayOrder;

            // 1️⃣ Silinecek kategoriden büyük DisplayOrder değerine sahip kategorileri al
            var affectedCategories = await _categoryRepository
                .GetAllAsync(c => c.DisplayOrder > deletedOrder);

            // 2️⃣ Eğer bu kategoriler varsa, DisplayOrder'larını bir azalt
            if (affectedCategories != null && affectedCategories.Any())
            {
                foreach (var category in affectedCategories.OrderBy(c => c.DisplayOrder))
                {
                    category.DisplayOrder -= 1;
                    await _categoryRepository.UpdateAsync(category);
                }
            }

            // 3️⃣ Kategoriyi sil
            var deleteCategoryResult = await _categoryRepository.DeleteAsync(categoryId);

            return deleteCategoryResult > 0
                ? new SuccessResult(Messages.DeleteCategorySuccess)
                : new ErrorResult(Messages.DeleteCategoryError);
        }

        #endregion

        #region Helper Methods
        private static void CompleteUpdate(CategoryViewModel model, IDataResult<Category> categoryResult)
        {
            categoryResult.Data.Name = model.Name;
            categoryResult.Data.ImageUrl = model.ImageUrl;
        }

        private async Task<IResult> CheckIfCategoryNameAlreadyExistsAsync(CategoryViewModel model)
        {
            var categoryList = await _categoryRepository.GetAllAsync(c => true);
            if (categoryList is not null &&
                categoryList.Any(c => c.Name.ToLower() == model.Name.ToLower().Trim() && c.Id != model.Id))
            {
                return new ErrorResult(Messages.CategoryAlreadyExists);
            }
            return new SuccessResult();
        }

        private async Task<IResult> GetUpdateResultAsync(IDataResult<Category> categoryResult)
        {
            var updateResult = await _categoryRepository.UpdateAsync(categoryResult.Data);
            return updateResult > 0
                ? new SuccessResult(Messages.UpdateCategorySuccess)
                : new ErrorResult(Messages.UpdateCategoryError);
        }

        private async Task ShiftCategoriesAfterUpdate(int oldOrder, int newOrder)
        {
            if (newOrder < oldOrder) // 🔼 Yukarı taşınıyor
            {
                var affectedCategories = await _categoryRepository
                    .GetAllAsync(c => c.DisplayOrder >= newOrder && c.DisplayOrder < oldOrder);

                foreach (var category in affectedCategories.OrderBy(c => c.DisplayOrder))
                {
                    category.DisplayOrder += 1; // Diğerlerini aşağı kaydır
                    await _categoryRepository.UpdateAsync(category);
                }
            }
            else if (newOrder > oldOrder) // 🔽 Aşağı taşınıyor
            {
                var affectedCategories = await _categoryRepository
                    .GetAllAsync(c => c.DisplayOrder > oldOrder && c.DisplayOrder <= newOrder);

                foreach (var category in affectedCategories.OrderBy(c => c.DisplayOrder))
                {
                    category.DisplayOrder -= 1; // Diğerlerini yukarı kaydır
                    await _categoryRepository.UpdateAsync(category);
                }
            }
        }

        #endregion
    }
}
