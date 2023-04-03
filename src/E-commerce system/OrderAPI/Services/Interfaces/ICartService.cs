using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels.Cart;

namespace OrderAPI.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModelResponse> GetById(Guid id);
    Task<CartViewModelResponse> Create(Guid id);
    Task<CartViewModelResponse> ComputeTotalValue(Guid id);
    Task<CartViewModelResponse> Clear(Guid id);
    Task<CartViewModelResponse> Check(Cart cart);
}
