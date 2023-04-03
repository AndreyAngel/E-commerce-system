using AutoMapper;
using OrderAPI.Models.DataBase;
using Infrastructure.DTO;
using OrderAPI.Models.ViewModels.Cart;
using OrderAPI.Models.ViewModels.Order;

namespace OrderAPI.Helpers;

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

        CreateMap<OrderViewModelRequest, Order>();

        CreateMap<Order, OrderViewModelResponse>();

        CreateMap<Order, OrderListViewModelResponse>();

        CreateMap<OrderCartProductViewModelRequest, CartProduct>();
    }
}
