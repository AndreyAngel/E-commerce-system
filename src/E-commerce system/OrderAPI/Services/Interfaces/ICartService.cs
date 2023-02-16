using OrderAPI.Models;

namespace OrderAPI.Services.Interfaces;

public interface ICartService
{
    Task<Cart> GetById(int id);
    Task<Cart> Create(Cart cart);
    Task<Cart> ComputeTotalValue(int id);
    Task<Cart> Clear(int id);
    Task<Cart> Check(Cart cart);
}
