using Core.Pagination;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<PaginatedList<ProductListDto>> GetProductDtosAsync(Guid currentShopId, string? searchTerm, int page, int pageSize)
    {
 
        var query = from product in _dataContext.Products
                    join category in _dataContext.Categories on product.CategoryId equals category.Id
                    where product.ShopId == currentShopId &&
                    (string.IsNullOrEmpty(searchTerm) ||
                        product.Name.Contains(searchTerm) ||
                        category.Name.Contains(searchTerm) 
                     )
                    select new ProductListDto
                    {
                        ProductId = product.Id,
                        CategoryId = category.Id,
                        ProductName = product.Name,
                        CategoryName = category.Name,
                        ImageUrl = product.ImageUrl,
                    };

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<ProductListDto>(items, totalItems, page, pageSize);
    }
}
