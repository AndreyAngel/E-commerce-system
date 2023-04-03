using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels.Cart;

namespace OrderAPI.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModel> GetById(Guid id);
    Task<CartViewModel> Create(Guid id);
    Task<CartViewModel> ComputeTotalValue(Guid id);
    Task<CartViewModel> Clear(Guid id);
    Task<CartViewModel> Check(Cart cart);
}
