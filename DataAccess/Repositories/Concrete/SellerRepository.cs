using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class SellerRepository(ApplicationDbContext context) : BaseRepository<Seller>(context), ISellerRepository
{ }