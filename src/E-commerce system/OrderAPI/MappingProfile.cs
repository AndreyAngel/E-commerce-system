using AutoMapper;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels;

namespace OrderAPI;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartProductViewModel, CartProduct>();

        CreateMap<CartProduct, CartProductViewModel>();

        CreateMap<CartViewModel, Cart>();

        CreateMap<Cart, CartViewModel>();
    }
}
