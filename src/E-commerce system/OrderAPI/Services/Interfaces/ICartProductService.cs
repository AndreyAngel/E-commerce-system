using OrderAPI.Models;

namespace OrderAPI.Services.Interfaces;

public interface ICartProductService
{
    Task<CartProduct> Create(CartProduct cartProduct);
    Task<CartProduct> Update(CartProduct cartProduct);
    Task Delete(int id);
}
