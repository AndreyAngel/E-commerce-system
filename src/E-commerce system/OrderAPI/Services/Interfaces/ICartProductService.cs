using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels.Cart;

namespace OrderAPI.Services.Interfaces;

public interface ICartProductService
{
    Task<CartProductViewModelResponse> Create(CartProduct cartProduct);
    Task<CartProduct> Update(CartProduct cartProduct);
    Task Delete(Guid id);
}
