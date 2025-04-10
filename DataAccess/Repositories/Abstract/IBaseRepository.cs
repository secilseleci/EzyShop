﻿using Azure;
using Core.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Entities.Abstract;
using System;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract;

public interface IBaseRepository<T> where T : class, IBaseEntity,IAuditable
{
    Task<int> CreateAsync(T entity);
    Task<int> CreateRangeAsync(IEnumerable<T> entities);


    Task<int> SoftDeleteAsync(Guid Id);
    Task<int> SoftDeleteRangeAsync(IEnumerable<T> entities);

    Task<int> HardDeleteAsync(Guid id);
    Task<int> HardDeleteRangeAsync(IEnumerable<T> entities);

    Task<int> UpdateAsync(T entity);
    Task<int> UpdateRangeAsync(IEnumerable<T> entities);

    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>?> GetWhereAsync(Expression<Func<T, bool>> predicate);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<PaginatedList<T>> GetPaginatedAsync(
         Expression<Func<T, bool>> predicate,
         int page,
         int pageSize,
         Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
}
