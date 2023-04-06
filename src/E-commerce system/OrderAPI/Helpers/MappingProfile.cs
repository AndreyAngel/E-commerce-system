using AutoMapper;
using Infrastructure.DTO;
using OrderAPI.Models.DTO.Cart;
using OrderAPI.Models.DTO.Order;
using OrderAPI.DataBase.Entities;
using OrderAPI.Models;

namespace OrderAPI.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cart, CartDomainModel>();

        CreateMap<CartDomainModel, Cart>();

        CreateMap<CartProductDomainModel, CartProduct>();

        CreateMap<CartProduct, CartProductDomainModel>();

        CreateMap<CartDTOResponse, CartDomainModel>();

        CreateMap<CartDomainModel, CartDTOResponse>();

        CreateMap<CartProductDTORequest, CartProductDomainModel>();

        CreateMap<CartProductDomainModel, CartProductDTOResponse>();

        CreateMap<CartProduct, CartProductDTOResponse>();

        CreateMap<CartProductDTOResponse, CartProductDomainModel>();

        CreateMap<ProductDTORabbitMQ, ProductDomainModel>();

        CreateMap<ProductDomainModel, ProductDTO>();

        CreateMap<CartProduct, OrderProduct>();

        CreateMap<OrderDTORequest, Order>()
            .ForMember(dst => dst.OrderProducts, opt => opt.MapFrom(src => src.CartProducts));

        CreateMap<Order, OrderDTOResponse>()
            .ForMember(dst => dst.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));

        CreateMap<Order, OrderListDTOResponse>();

        CreateMap<OrderCartProductDTORequest, OrderProduct>();

        CreateMap<OrderProduct, OrderProductDTO>();
    }
}
