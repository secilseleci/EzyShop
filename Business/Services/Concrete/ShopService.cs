using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using Models.ViewModels;

public class ShopService : IShopService
{
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;

    public ShopService(IShopRepository shopRepository, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _mapper = mapper;
    }

    public async Task<IDataResult<ShopViewModel>> GetShopBySellerIdAsync(Guid sellerId)
    {
        var shop = await _shopRepository.GetBySellerIdAsync(sellerId);
        if (shop == null)
        {
            return new ErrorDataResult<ShopViewModel>(Messages.ShopNotFound);
        }

        var mappedShop = _mapper.Map<ShopViewModel>(shop);
        return new SuccessDataResult<ShopViewModel>(mappedShop);
    }

    public async Task<IResult> CreateShopAsync(Shop entity)
    {
        var existingShop = await _shopRepository.GetAllAsync(s => s.Name.ToLower() == entity.Name.ToLower());
        if (existingShop != null && existingShop.Any())
        {
            return new ErrorResult(Messages.ShopAlreadyExists);
        }


        var result = await _shopRepository.CreateAsync(entity);
        return result > 0
            ? new SuccessResult(Messages.CreateShopSuccess)
            : new ErrorResult(Messages.CreateShopError);
    }

    
 
}
