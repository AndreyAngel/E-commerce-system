using OrderAPI.Models.ViewModels;
using OrderAPI.Models.DataBase;

namespace OrderAPI.Services.Interfaces;

public interface ICartProductService
{
    Task<CartProductViewModel> Create(CartProduct cartProduct);
    Task<CartProductViewModel> Update(CartProductViewModel cartProduct);
    Task Delete(int id);
}
