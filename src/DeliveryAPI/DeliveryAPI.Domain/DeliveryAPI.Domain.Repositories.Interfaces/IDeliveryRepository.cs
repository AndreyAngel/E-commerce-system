using DeliveryAPI.Domain.Entities;
using System.Linq.Expressions;

namespace DeliveryAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the delivery repository class containing methods for interaction with the database
/// </summary>
public interface IDeliveryRepository : IGenericRepositoty<Delivery>
{
    /// <summary>
    /// Include data from another database table
    /// </summary>
    /// <param name="includeProperties"> Include properties </param>
    /// <returns> A new query with the released data included </returns>
    IQueryable<Delivery> Include(params Expression<Func<Delivery, object>>[] includeProperties);
}
