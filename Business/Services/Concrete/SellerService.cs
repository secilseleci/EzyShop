using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Pagination;
using Core.Security;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.ViewModels.Seller;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class SellerService : BaseService, ISellerService
{
    private readonly ISellerRepository _sellerRepo;

    public SellerService(
     ISellerRepository sellerRepo,
     IMapper mapper,
     IConfiguration config,
     ICurrentUserService currentUser
    ) : base(mapper, config, currentUser)
    {
        _sellerRepo = sellerRepo;
    }
    #region Delete Seller
    
    public async Task<IResult> DeleteSellerAsync(Guid sellerId)
    {
        var exists = await _sellerRepo.ExistsAsync(s => s.Id == sellerId && !s.IsDeleted);
        if (!exists)
            return new ErrorResult(Messages.SellerNotFound);

        var affectedRows = await _sellerRepo.SoftDeleteAsync(sellerId);

        return affectedRows > 0
        ? new SuccessResult(Messages.DeleteSellerSuccess)
        : new ErrorResult(Messages.DeleteSellerError);
    }
    #endregion
    #region List Seller
  
    public async Task<IDataResult<PaginatedList<SellerListViewModel>>> GetPaginatedSellersAsync(
        int page,
        int pageSize,
        string? searchTerm = null)
    {
        Expression<Func<Seller, bool>> predicate;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var loweredSearchTerm = searchTerm.ToLower().Trim();

            ApplicationStatus parsedStatus = default;
            bool isEnumMatch = Enum.TryParse<ApplicationStatus>(
                searchTerm, true, out parsedStatus
            );

            predicate = s =>
                (s.User != null && (
                    (s.User.Name + " " + s.User.Surname).ToLower().Contains(loweredSearchTerm) ||
                    s.User.Email!.ToLower().Contains(loweredSearchTerm) ||
                    s.User.PhoneNumber!.ToLower().Contains(loweredSearchTerm)
                )) ||

                (s.SellerApplication != null && (
                    (isEnumMatch && s.SellerApplication.Status == parsedStatus) ||
                    s.SellerApplication.ShopAddress.ToLower().Contains(loweredSearchTerm) ||
                    s.SellerApplication.TaxNumber.ToLower().Contains(loweredSearchTerm) ||
                    s.SellerApplication.ShopName.ToLower().Contains(loweredSearchTerm)
                ))  ;
        }
        else
        {
            predicate = s => true;
        }

        var paginatedSellers = await _sellerRepo.GetPaginatedAsync(
            predicate,
            page,
            pageSize,
            q => q
                .Include(s => s.User)
                .Include(s => s.SellerApplication)        
                );

        var viewModels = Mapper.Map<IEnumerable<SellerListViewModel>>(paginatedSellers.Items);

        var result = new PaginatedList<SellerListViewModel>(
            viewModels,
            paginatedSellers.TotalItems,
            paginatedSellers.Page,
            paginatedSellers.PageSize
        );

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<SellerListViewModel>>(result)
            : new ErrorDataResult<PaginatedList<SellerListViewModel>>(Messages.EmptySellerList);
    }  
    #endregion
    #region GetById Seller
    public async Task<IDataResult<Seller>> GetSellerByIdAsync(Guid sellerId)
    {
        var seller = await _sellerRepo.GetByIdAsync(sellerId);
        return seller is not null
            ? new SuccessDataResult<Seller>(seller)
            : new ErrorDataResult<Seller>(Messages.SellerNotFound);
    }
    #endregion
    #region GetByUserId
    public async Task<IDataResult<SellerViewModel>> GetSellerByUserId(Guid userId)
    {
        var seller = await _sellerRepo.GetSellerByUserIdAsync(userId);

        if (seller == null)
            return new ErrorDataResult<SellerViewModel>(Messages.SellerNotFound);

        var vm = Mapper.Map<SellerViewModel>(seller);
        return new SuccessDataResult<SellerViewModel>(vm);
    }
    #endregion
}
