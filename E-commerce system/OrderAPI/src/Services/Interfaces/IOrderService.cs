using OrderAPI.DataBase.Entities;
using OrderAPI.Models.DTO.Order;

namespace OrderAPI.Services.Interfaces;

/// <summary>
/// Interface for class providing the APIs for managing cart in a persistence store.
/// </summary>
public interface IOrderService : IDisposable
{
    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns> Orders list </returns>
    List<Order> GetAll();

    /// <summary>
    /// Get order by Id
    /// </summary>
    /// <param name="id"> The order Id </param>
    /// <returns> Order </returns>
    Order GetById(Guid id);

    /// <summary>
    /// Get order by filters
    /// </summary>
    /// <param name="filter"> Filters </param>
    /// <returns> Orders list </returns>
    List<Order> GetByFilter(OrderFilterDTORequest filter);

    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="order"> The new order </param>
    /// <returns> Task object contaning of created order </returns>
    Task<Order> Create(Order order);

    /// <summary>
    /// Update order
    /// </summary>
    /// <param name="order"> The order </param>
    /// <returns> Task object containing of updated order </returns>
    Task<Order> Update(Order order);

    /// <summary>
    /// The order is ready 
    /// </summary>
    /// <param name="id"> The order Id </param>
    /// <returns> Task object containing order </returns>
    Task<Order> IsReady(Guid id);

    /// <summary>
    /// The order is received
    /// </summary>
    /// <param name="id"> The order Id </param>
    /// <returns> Task object containing order </returns>
    Task<Order> IsReceived(Guid id);

    /// <summary>
    /// Cansel the order
    /// </summary>
    /// <param name="id"> The order Id </param>
    /// <returns> Task object containing order </returns>
    Task<Order> Cancel(Guid id);

    /// <summary>
    /// Pay the order
    /// </summary>
    /// <param name="id"> The order Id </param>
    /// <returns> Task object containing order </returns>
    Task<Order> IsPaymented(Guid id);
}
