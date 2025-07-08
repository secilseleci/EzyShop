﻿using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>> predicate);
}
