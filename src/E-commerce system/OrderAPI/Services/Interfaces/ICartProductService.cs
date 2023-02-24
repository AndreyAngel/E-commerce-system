using OrderAPI.Models.ViewModels;
using OrderAPI.Models.DataBase;

namespace OrderAPI.Services.Interfaces;

public interface ICartProductService
{
    Task<CartProductViewModelResponse> Create(CartProduct cartProduct);
    Task<CartProduct> Update(CartProduct cartProduct);
    Task Delete(int id);
}
