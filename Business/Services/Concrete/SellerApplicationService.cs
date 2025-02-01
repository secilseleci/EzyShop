using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Concrete
{
    public class SellerApplicationService:ISellerApplicationService
    {
        private readonly ISellerApplicationRepository _sellerApplicationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public SellerApplicationService(
            ISellerApplicationRepository sellerApplicationRepository, 
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _sellerApplicationRepository = sellerApplicationRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        #region Read
        public async Task<IDataResult<SellerApplication>> GetSellerApplicationByIdAsync(Guid id)
        {
            var sellerApplication = await _sellerApplicationRepository.GetByIdAsync(id);
            return sellerApplication is not null
                ? new SuccessDataResult<SellerApplication>(sellerApplication)
                : new ErrorDataResult<SellerApplication>(Messages.ApplicationNotFound);
        }
        public async Task<IDataResult<IEnumerable<SellerApplication>>> GetAllSellerApplicationsAsync(Expression<Func<SellerApplication, bool>> predicate)
        {
            var applicationList = await _sellerApplicationRepository.GetAllAsync(predicate);
            return applicationList is not null && applicationList.Any()
                ? new SuccessDataResult<IEnumerable<SellerApplication>>(applicationList)
                : new ErrorDataResult<IEnumerable<SellerApplication>>(Messages.EmptySellerApplicationList);
        }



        #endregion

        #region Act

        public async Task<IResult> ApproveSellerAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> RejectSellerAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Create

        public async Task<IResult> CreateSellerApplicationAsync(BecomeSellerViewModel model)
        {
          
            var existingApplication = await _sellerApplicationRepository.GetAllAsync(a => a.Email == model.Email && a.Status == ApplicationStatus.Pending);
            if (existingApplication != null && existingApplication.Any())
            {
                return new ErrorResult(Messages.ExistingSellerApplicationtError);
            }
            var sellerApplication = _mapper.Map<SellerApplication>(model);
            sellerApplication.Status = ApplicationStatus.Pending;

            var result = await _sellerApplicationRepository.CreateAsync(sellerApplication);

            return result > 0
               ? new SuccessResult(Messages.CreateSellerApplicationSuccess)
               : new ErrorResult(Messages.CreateSellerApplicationtError);
        }
        #endregion


        #region Delete
        public async Task<IResult> DeleteSellerApplicationAsync(Guid Id)
        {
            var deleteAppResult = await _sellerApplicationRepository.DeleteAsync(Id);
            return deleteAppResult > 0
                ? new SuccessResult(Messages.DeleteSellerApplicationSuccess)
                : new ErrorResult(Messages.DeleteSellerApplicationError);
        }

        #endregion


    }
}
