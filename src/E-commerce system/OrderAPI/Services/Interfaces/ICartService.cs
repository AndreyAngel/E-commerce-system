using OrderAPI.Models.ViewModels;
using OrderAPI.Models.DataBase;

namespace OrderAPI.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModel> GetById(int id);
    Task<CartViewModel> Create(int id);
    Task<CartViewModel> ComputeTotalValue(int id);
    Task<CartViewModel> Clear(int id);
    Task<CartViewModel> Check(Cart cart);
}
