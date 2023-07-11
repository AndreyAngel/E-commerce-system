namespace StoreAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the generic repository class containing methods for interaction with the database
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
{ }
