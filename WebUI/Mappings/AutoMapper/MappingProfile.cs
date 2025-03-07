using AutoMapper;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Category;
using Models.ViewModels.Order;
using Models.ViewModels.Product;
using Models.ViewModels.Shop;
using Models.ViewModels.User;

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

            #region ShoppingCartItem → OrderItemViewModel
            CreateMap<ShoppingCartItem, OrderItemViewModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Product.Color))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price * src.Count))
                .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Product.Shop.Id))
                .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Product.Shop.Name));
            #endregion


            #region ShoppingCartItem → ShopOrderViewModel
            CreateMap<ShoppingCartItem, ShopOrderViewModel>()
                .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Product.ShopId))
                .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Product.Shop.Name))
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
            #endregion

            #region AppUser → SummaryViewModel
            CreateMap<AppUser, SummaryViewModel>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
            #endregion

            #region SummaryViewModel → Order
            CreateMap<SummaryViewModel, Order>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Pending))  
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
            #endregion
            #region Order → OrderViewModel
            CreateMap<Order, OrderViewModel>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
            .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.Customer.Address))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))   
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));   
            #endregion


            #region OrderItem → OrderItemViewModel
            CreateMap<OrderItem, OrderItemViewModel>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.ProductPrice))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))   
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))  
            .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            #endregion  
        }
    }
}