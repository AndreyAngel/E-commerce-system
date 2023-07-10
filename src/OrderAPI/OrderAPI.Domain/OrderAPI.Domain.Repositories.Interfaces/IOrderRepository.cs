using OrderAPI.Domain.Entities;
using System.Linq.Expressions;

namespace OrderAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the order repository class containing methods for interaction with the database
/// </summary>
public interface IOrderRepository : IGenericRepository<Order>
{
    /// <summary>
    /// Get all entity objects
    /// </summary>
    /// <returns> Query of entity objects </returns>
    IQueryable<Order> GetAll();

    /// <summary>
    /// Get entity by Id
    /// </summary>
    /// <param name="Id"> Object Id </param>
    /// <returns> One entity object </returns>
    Order? GetById(Guid Id);

    /// <summary>
    /// Include data from another database table
    /// </summary>
    /// <param name="includeProperties"> Include properties </param>
    /// <returns> A new query with the released data included </returns>
    IQueryable<Order> Include(params Expression<Func<Order, object>>[] includeProperties);
}
