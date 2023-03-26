using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels.Cart;

namespace OrderAPI.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModel> GetById(int id);
    Task<CartViewModel> Create(int id);
    Task<CartViewModel> ComputeTotalValue(int id);
    Task<CartViewModel> Clear(int id);
    Task<CartViewModel> Check(Cart cart);
}
