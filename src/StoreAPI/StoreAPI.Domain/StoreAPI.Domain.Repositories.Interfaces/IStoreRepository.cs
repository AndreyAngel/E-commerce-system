using StoreAPI.Domain.Entities;

namespace StoreAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the store repository class containing methods for interaction with the database
/// </summary>
public interface IStoreRepository : IGenericRepository<Store>
{
    /// <summary>
    /// Get all entity objects
    /// </summary>
    /// <returns> Query of entity objects </returns>
    IQueryable<Store> GetAll();

    /// <summary>
    /// Get entity by Id
    /// </summary>
    /// <param name="Id"> Object Id </param>
    /// <returns> One entity object </returns>
    Store? GetById(Guid Id);

    /// <summary>
    /// Create a new entity object
    /// </summary>
    /// <param name="entity"> New entity object </param>
    /// <returns> New entity object </returns>
    Task AddAsync(Store entity);

    /// <summary>
    /// Update a entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object containing updated entity object </returns>
    Task UpdateAsync(Store entity);

    /// <summary>
    /// Remove entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object </returns>
    Task RemoveAsync(Store entity);
}
