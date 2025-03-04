using AutoMapper;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Mappings.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Category → CategoryViewModel
            CreateMap<Category, CategoryViewModel>()
                       .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl)) ;

            CreateMap<CategoryViewModel, Category>();
            #endregion

            #region Product → ProductViewModel
            CreateMap<ProductViewModel, Product>().ReverseMap();
            #endregion

            #region AppUser → RegisterViewModel
            CreateMap<RegisterViewModel, AppUser>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FullName.Trim().Replace(" ", "-")));
            #endregion
            #region AppUser → UserProfileViewModel
            CreateMap<AppUser, UserProfileViewModel>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());
            #endregion
            #region AppUser → UserViewModel
            CreateMap<AppUser, UserViewModel>()
            .ForMember(dest => dest.Role, opt => opt.Ignore()).ReverseMap();

            #endregion

            #region SellerApplication → SellerRegistrationViewModel
            CreateMap<SellerRegistrationViewModel, SellerApplication>()
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApplicationStatus.Pending)) 
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())) 
           .ForMember(dest => dest.UserId, opt => opt.Ignore());
            #endregion

            #region Shop → ShopViewModel
            CreateMap<ShopViewModel, Shop>().ReverseMap();

            #endregion

            #region ShoppingCartItem → ShoppingCartItemViewModel
            CreateMap<ShoppingCartItem, ShoppingCartItemViewModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Product.Color))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Product.ShopId))  // 🏪 Shop ID maplendi
                .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Product.Shop.Name));
            #endregion
            #region ShoppingCartItemViewModel → OrderItemSummaryViewModel
            CreateMap<ShoppingCartItemViewModel, OrderItemSummaryViewModel>()
           .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
           .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price))
           .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
           .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
            #endregion
            #region ShoppingCartItem → ShopOrderSummaryViewModel
            CreateMap<ShoppingCartItem, ShopOrderSummaryViewModel>()
                .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Product.ShopId))
                .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Product.Shop.Name));
            #endregion}
            #region AppUser → OrderSummaryViewModel
            CreateMap<AppUser, OrderSummaryViewModel>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
            #endregion 
        }
    }
}