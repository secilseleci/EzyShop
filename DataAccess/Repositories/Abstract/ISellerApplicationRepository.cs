using Models.Entities.Concrete;


namespace DataAccess.Repositories.Abstract
{
    public interface ISellerApplicationRepository:IBaseRepository<SellerApplication>
    {
        Task<IEnumerable<SellerApplication>> GetApplicationsByStatusAsync(ApplicationStatus status);
      
    }
}
