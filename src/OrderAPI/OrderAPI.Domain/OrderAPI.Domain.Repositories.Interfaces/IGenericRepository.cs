namespace OrderAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the generic repository class containing methods for interaction with the database
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
{
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
}
