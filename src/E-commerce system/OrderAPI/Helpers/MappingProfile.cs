using AutoMapper;
using Infrastructure.DTO;
using OrderAPI.Models.DTO.Cart;
using OrderAPI.Models.DTO.Order;
using OrderAPI.DataBase.Entities;

namespace OrderAPI.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartDTOResponse, Cart>();

        CreateMap<Cart, CartDTOResponse>();

        CreateMap<CartProductDTORequest, CartProduct>();

        CreateMap<CartProduct, CartProductDTOResponse>();

        CreateMap<CartProductDTOResponse, CartProduct>();

        CreateMap<ProductDTORabbitMQ, ProductDTO>();

        CreateMap<OrderDTORequest, Order>();

        CreateMap<Order, OrderDTOResponse>();

        CreateMap<Order, OrderListDTOResponse>();

        CreateMap<OrderCartProductDTORequest, CartProduct>();
    }
}
