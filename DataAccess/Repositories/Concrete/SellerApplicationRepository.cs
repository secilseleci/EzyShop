using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete
{
    public class SellerApplicationRepository(ApplicationDbContext context) : BaseRepository<SellerApplication>(context), ISellerApplicationRepository
    {
        public async Task<IEnumerable<SellerApplication>> GetApplicationsByStatusAsync(ApplicationStatus status)
        {
            return await _dataContext.SellerApplications
                .Where(sa => sa.Status == status && !sa.IsDeleted)
                .ToListAsync();
        }

      
 
    }

}
