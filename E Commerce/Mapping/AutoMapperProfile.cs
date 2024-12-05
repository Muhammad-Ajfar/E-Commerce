using AutoMapper;
using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterUserDTO, User>();
            CreateMap<User, UserResponseDTO>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<User, UserViewDTO>();

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Product, ProductGetDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<ProductDTO, Product>();

            CreateMap<WishList, WishListDTO>().ReverseMap();

        }
    }
}
