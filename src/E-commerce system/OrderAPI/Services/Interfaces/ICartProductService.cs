using OrderAPI.DataBase.Entities;
using OrderAPI.Models.DTO.Cart;

namespace OrderAPI.Services.Interfaces;

public interface ICartProductService
{
    Task<CartProductDTOResponse> Create(CartProduct cartProduct);
    Task<CartProduct> Update(CartProduct cartProduct);
    Task Delete(Guid id);
}
