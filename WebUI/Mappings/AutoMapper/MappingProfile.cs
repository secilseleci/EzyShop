using AutoMapper;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Category;
using Models.ViewModels.Customer;
using Models.ViewModels.Product;
using Models.ViewModels.Seller;
using Models.ViewModels.SellerApplication;
using Models.ViewModels.Shop;

namespace WebUI.Mappings.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Category
        CreateMap<Category,CategoryViewModel>()
 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => (src.Name ?? string.Empty).Trim())).ReverseMap();

        #endregion

        #region Product 
        CreateMap<ProductCreateViewModel, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())  
            .ForMember(dest => dest.ShopId, opt => opt.Ignore())  
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? string.Empty))
            .ForMember(dest => dest.IsSoldOut, opt => opt.Ignore())  
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore())  
            .ForMember(dest => dest.CartLines, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
            .ForMember(dest => dest.Shop, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());
        
        CreateMap<Product, ProductUpdateViewModel>().ReverseMap();
       
        CreateMap<Product, ProductSellerViewModel>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "N/A"))
        .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.IsActive ? "Active" : "Passive"))
        .ForMember(dest => dest.StockStatus, opt => opt.MapFrom(src => src.Stock <= 0 ? "Out of Stock" : "In Stock"));


        CreateMap<Product, ProductCustomerViewModel>();
        CreateMap<Product, ProductFilterViewModel>();
        CreateMap<Product, ProductDetailViewModel>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
        .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop.Name));

        #endregion

        #region SellerApplication  

        CreateMap<SellerApplication, SellerApplicationViewModel>().ReverseMap();
        #endregion

        #region Shop  
        CreateMap<Shop, ShopViewModel>()
          .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller.User.FullName))
          .ReverseMap();

        #endregion

        #region AppUser for Register Customer
        CreateMap<RegisterViewModel, AppUser>()
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' })[0]))
        .ForMember(dest => dest.Surname, opt => opt.MapFrom(src =>
            src.FullName.Split(new[] { ' ' }).Length > 1
                ? string.Join(" ", src.FullName.Split(new[] { ' ' }).Skip(1))
                : ""))
        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ContactNumber))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        #endregion
        
        #region Customer for List
        CreateMap<Customer, CustomerListViewModel>()
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}".Trim()))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));
        #endregion
        #region Seller for List
        CreateMap<Seller, SellerListViewModel>()
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}".Trim()))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
        .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.SellerApplication != null ? src.SellerApplication.TaxNumber : null))
        .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.SellerApplication != null ? src.SellerApplication.ShopName : null))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.SellerApplication.Status));
        #endregion

        #region Seller for Profile
        CreateMap<Seller, SellerViewModel>()
          .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}".Trim()))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
          .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
          .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.Shop != null ? src.Shop.TaxNumber : null))
          .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop != null ? src.Shop.Name : null))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Shop != null ? src.Shop.Status : default))
          .ForMember(dest => dest.ShopAddress, opt => opt.MapFrom(src => src.Shop != null ? src.Shop.ShopAddress : null))
          .ForMember(dest => dest.OpeningDate, opt => opt.MapFrom(src => src.Shop != null ? src.Shop.CreatedAt : (DateTime?)null)).ReverseMap();


        #endregion

        #region  Create SellerApplication 
        CreateMap<SellerApplicationCreateViewModel, SellerApplication>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApplicationStatus.Pending))
        .ForMember(dest => dest.UserId, opt => opt.Ignore())
        .ForMember(dest => dest.SellerId, opt => opt.Ignore());
        #endregion

        #region SellerApplication for List
        CreateMap<SellerApplication, SellerApplicationViewModel>()
        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}".Trim()))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));  
        #endregion



         

  
  
    }
}