using StoreAPI.Domain.Entities;
using System.Linq.Expressions;

namespace StoreAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the stock repository class containing methods for interaction with the database
/// </summary>
public interface IStockRepository : IGenericRepository<Stock>
{
    /// <summary>
    /// Include data from another database table
    /// </summary>
    /// <param name="includeProperties"> Include properties </param>
    /// <returns> A new query with the released data included </returns>
    IQueryable<Stock> Include(params Expression<Func<Stock, object>>[] includeProperties);

    /// <summary>
    /// Create a new entity object
    /// </summary>
    /// <param name="entity"> New entity object </param>
    /// <returns> New entity object </returns>
    Task AddAsync(Stock entity);

    /// <summary>
    /// Remove entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object </returns>
    Task RemoveAsync(Stock entity);
}
