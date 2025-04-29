using Core.Constants;
using Core.Pagination;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<PaginatedList<ProductListDto>> GetProductDtosAsync(ProductStatus status, Guid shopId, string? searchTerm, int page, int pageSize)
    {

        var filteredProducts = _dataContext.Products.Where(GetStatusFilter(status, shopId));

        var query =
            from product in filteredProducts
            join category in _dataContext.Categories on product.CategoryId equals category.Id

            where
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

    private static Expression<Func<Product, bool>> GetStatusFilter(ProductStatus status, Guid shopId)
    {
        return status switch
        {
            ProductStatus.Available => product => !product.IsDeleted && product.IsActive && product.ShopId == shopId && product.Stock > 0,
            ProductStatus.SoldOut => product => !product.IsDeleted && !product.IsActive && product.ShopId == shopId && product.Stock <= 0,
            _ => product => false
        };
    }
    public async Task<ProductDetailsDto> GetProductDetailsDtosAsync(Guid shopId, Guid productId)
    {
        
        var result = await (from p in _dataContext.Products
                            join c in _dataContext.Categories
                                on p.CategoryId equals c.Id
                            where p.Id == productId && p.ShopId == shopId
                            select new ProductDetailsDto
                            {
                                ProductId = p.Id,
                                CategoryId = c.Id,
                                CategoryName = c.Name,
                                ProductName = p.Name,
                                ImageUrl = p.ImageUrl,
                                Price = p.Price,
                                Color = p.Color,
                                Stock = p.Stock
                            })
                  .FirstOrDefaultAsync();

        return result!;
    }
}