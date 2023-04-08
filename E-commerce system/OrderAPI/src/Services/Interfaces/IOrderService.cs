using OrderAPI.DataBase.Entities;
using OrderAPI.Models.DTO.Order;

namespace OrderAPI.Services.Interfaces;

public interface IOrderService : IDisposable
{
    List<Order> GetAll();

    Order GetById(Guid id);

    List<Order> GetByFilter(OrderFilterDTORequest filter);

    Task<Order> Create(Order order);

    Task<Order> Update(Order order);

    Task<Order> IsReady(Guid id);

    Task<Order> IsReceived(Guid id);

    Task<Order> Cancel(Guid id);

    Task<Order> IsPaymented(Guid id);
}
