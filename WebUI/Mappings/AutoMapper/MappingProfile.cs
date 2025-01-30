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
            CreateMap<Category, CategoryViewModel>()
                       .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl)) ;

            CreateMap<CategoryViewModel, Category>();
            CreateMap<ProductViewModel, Product>().ReverseMap();
            CreateMap<RegisterViewModel, AppUser>()
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
          .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FullName.Trim().Replace(" ", "-")));

            CreateMap<AppUser, UserViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()).ReverseMap();
        }
    }
}
