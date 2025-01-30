using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete
{
    public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
    {
    }
}
