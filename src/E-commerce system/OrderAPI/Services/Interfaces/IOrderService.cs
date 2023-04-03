using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels.Order;

namespace OrderAPI.Services.Interfaces;

public interface IOrderService
{
    List<Order> GetAll();

    Order GetById(int id);

    List<Order> GetByFilter(OrderFilterViewModelRequest filetr);

    Task<Order> Create(Order order);

    Task<Order> Update(Order order);

    Task<Order> IsReady(int id);

    Task<Order> IsReceived(int id);

    Task<Order> Cancel(int id);

    Task<Order> IsPaymented(int id);
}
