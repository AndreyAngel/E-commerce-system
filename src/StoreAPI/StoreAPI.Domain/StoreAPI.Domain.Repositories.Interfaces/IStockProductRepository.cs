using StoreAPI.Domain.Entities;

namespace StoreAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the stock product repository class containing methods for interaction with the database
/// </summary>
public interface IStockProductRepository : IGenericRepository<StockProduct>
{
    /// <summary>
    /// Get all entity objects
    /// </summary>
    /// <returns> Query of entity objects </returns>
    IQueryable<StockProduct> GetAll();

    /// <summary>
    /// Update a entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object containing updated entity object </returns>
    Task UpdateAsync(StockProduct entity);

    /// <summary>
    /// Remove entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object </returns>
    Task RemoveAsync(StockProduct entity);
}
