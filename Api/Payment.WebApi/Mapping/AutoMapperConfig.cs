using AutoMapper;
using Payment.DtoLayer.Dtos.AddressDtos;
using Payment.DtoLayer.Dtos.AppUserDtos;
using Payment.DtoLayer.Dtos.CategoryDtos;
using Payment.DtoLayer.Dtos.ProductDtos;
using Payment.DtoLayer.Dtos.RegisterDtos;
using Payment.DtoLayer.Dtos.WishlistDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.WebApi.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ResultAppUserDto, AppUser>().ReverseMap();
            CreateMap<GetByIdAppUserDto, AppUser>().ReverseMap();
            CreateMap<UpdateAppUserDto, AppUser>().ReverseMap().ForAllMembers(opt =>
            opt.Condition((src, dest, srcMember) => srcMember != null && !string.IsNullOrWhiteSpace(srcMember.ToString())));



            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
            CreateMap<ResultCategoryDto, Category>().ReverseMap();
            CreateMap<GetByIdCategoryDto, Category>().ReverseMap();
            CreateMap<ResultCategoryByUserEmailDto, Category>().ReverseMap();

            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();
            CreateMap<ResultProductDto, Product>().ReverseMap();
            CreateMap<GetByIdProductDto, Product>().ReverseMap();
            CreateMap<ResultProductByUserEmailDto, Product>().ReverseMap();

            CreateMap<CreateAddressDto, Address>().ReverseMap();
            CreateMap<UpdateAddressDto, Address>().ReverseMap();
            CreateMap<ResultAddressDto, Address>().ReverseMap().ForMember(dest => dest.AppUserName, opt => opt.MapFrom(src => src.CreateUser.Name + " " + src.CreateUser.Surname));
            CreateMap<GetByIdAddressDto, Address>().ReverseMap();

            CreateMap<UpdateWishlistDto, Wishlist>().ReverseMap();
            CreateMap<GetByIdWishlistDto, Wishlist>().ReverseMap();
            CreateMap<ResultWishlistDto, Wishlist>().ReverseMap();
            CreateMap<CreateWishlistDto,Wishlist>().ReverseMap();




            CreateMap<RegisterDto, AppUser>(MemberList.Source)
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
               .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => DateTime.UtcNow))
               .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => DateTime.UtcNow));

        }
    }
}
