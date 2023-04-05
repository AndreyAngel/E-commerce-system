using OrderAPI.Models.ViewModels.Cart;

namespace OrderAPI.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModelResponse> GetById(Guid id);

    Task Create(Guid id);

    Task<CartViewModelResponse> ComputeTotalValue(Guid id);

    Task<CartViewModelResponse> Clear(Guid id);
}
