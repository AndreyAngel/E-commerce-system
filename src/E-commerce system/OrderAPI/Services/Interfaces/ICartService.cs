using OrderAPI.Models.DTO.Cart;

namespace OrderAPI.Services.Interfaces;

public interface ICartService
{
    Task<CartDTOResponse> GetById(Guid id);

    Task Create(Guid id);

    Task<CartDTOResponse> ComputeTotalValue(Guid id);

    Task<CartDTOResponse> Clear(Guid id);
}
