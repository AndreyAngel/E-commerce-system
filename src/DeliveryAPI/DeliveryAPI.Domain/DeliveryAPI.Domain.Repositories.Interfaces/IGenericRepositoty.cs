namespace DeliveryAPI.Domain.Repositories.Interfaces;

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
    /// Create a new entity object
    /// </summary>
    /// <param name="entity"> New entity object </param>
    /// <returns> New entity object </returns>
    Task AddAsync(TEntity entity);
}
