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
            #region Category
            CreateMap<Category, CategoryViewModel>()
                       .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl)) ;

            CreateMap<CategoryViewModel, Category>();
            #endregion
            
            #region Product
            CreateMap<ProductViewModel, Product>().ReverseMap();
            #endregion

            #region AppUser
            CreateMap<RegisterViewModel, AppUser>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FullName.Trim().Replace(" ", "-")));
            
            CreateMap<AppUser, UserProfileViewModel>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<AppUser, UserViewModel>()
            .ForMember(dest => dest.Role, opt => opt.Ignore()).ReverseMap();
            #endregion

            CreateMap<SellerRegistrationViewModel, SellerApplication>()
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApplicationStatus.Pending)) 
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())) 
           .ForMember(dest => dest.UserId, opt => opt.Ignore()); 

            #region Shop
            CreateMap<ShopViewModel, Shop>().ReverseMap();

            #endregion

            #region ShoppingCartItem
            CreateMap<ShoppingCartItem, ShoppingCartItemViewModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Product.Color))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price)) ;
            #endregion
        }
    }
}
