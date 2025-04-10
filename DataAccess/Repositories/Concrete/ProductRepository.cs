﻿using Core.Pagination;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using System.Linq.Expressions;
using static Models.Entities.Concrete.Shop;

namespace DataAccess.Repositories.Concrete;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<Product?> GetProductWithIncludesAsync(Guid id)
    {
        return await _dataContext.Products
            .Include(p => p.Category)
            .Include(p => p.Shop).ThenInclude(s => s.Seller)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }
 
  
}
