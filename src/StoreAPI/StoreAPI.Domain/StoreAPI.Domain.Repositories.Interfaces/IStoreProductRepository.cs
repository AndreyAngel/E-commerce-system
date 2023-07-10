using StoreAPI.Domain.Entities;

namespace StoreAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the store product repository class containing methods for interaction with the database
/// </summary>
public interface IStoreProductRepository : IGenericRepository<StoreProduct>
{
    /// <summary>
    /// Get all entity objects
    /// </summary>
    /// <returns> Query of entity objects </returns>
    IQueryable<StoreProduct> GetAll();

    /// <summary>
    /// Update a entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object containing updated entity object </returns>
    Task UpdateAsync(StoreProduct entity);
}
