using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete
{
    public class SellerApplicationRepository(ApplicationDbContext context) : BaseRepository<SellerApplication>(context), ISellerApplicationRepository
    {


      
    }
   
}
