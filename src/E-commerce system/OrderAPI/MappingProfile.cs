using AutoMapper;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels;
using Infrastructure.DTO;

namespace OrderAPI;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartViewModel, Cart>();

        CreateMap<Cart, CartViewModel>();

        CreateMap<CartProductViewModelRequest, CartProduct>();

        CreateMap<CartProduct, CartProductViewModelResponse>();

        CreateMap<CartProductViewModelResponse, CartProduct>();

        CreateMap<ProductDTO, ProductViewModel>();
    }
}
