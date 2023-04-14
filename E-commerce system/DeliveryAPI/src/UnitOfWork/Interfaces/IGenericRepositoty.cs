using System.Linq.Expressions;

namespace DeliveryAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the generic repository class containing methods for interaction with the database
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IGenericRepositoty<TEntity> : IDisposable where TEntity : class
{
    /// <summary>
    /// Get all entity objects
    /// </summary>
    /// <returns> List of entity objects </returns>
    IEnumerable<TEntity> GetAll();

    /// <summary>
    /// Get entity by Id
    /// </summary>
    /// <param name="Id"> Object Id </param>
    /// <returns> One entity object </returns>
    TEntity? GetById(Guid Id);

    /// <summary>
    /// Include data from another database table
    /// </summary>
    /// <param name="includeProperties"> Include properties </param>
    /// <returns> A new query with the released data included </returns>
    IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);

    /// <summary>
    /// Create a new entity object
    /// </summary>
    /// <param name="entity"> New entity object </param>
    /// <returns> New entity object </returns>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Update a entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object containing updated entity object </returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Remove entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object </returns>
    Task RemoveAsync(TEntity entity);
}
