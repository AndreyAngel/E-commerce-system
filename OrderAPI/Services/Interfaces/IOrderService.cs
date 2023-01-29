using OrderAPI.Models;

namespace OrderAPI.Services.Interfaces;

interface IOrderService
{
    Task<List<Order>> GetAll();
    Task<Order> GetById(int id);
    Task<List<Order>> GetByUserId(int userId);
    Task<Order> Create(Order order);
    Task<Order> Update(Order order);
}
