namespace StoreAPI.UnitOfWork.Interfaces;

/// <summary>
/// An interface for class that implements the unit of work pattern
/// and contains all entity repositories to create a single database context.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Interface for the store repository class containing methods for interaction with the database
    /// </summary>
    public IStoreRepository Stores { get; }

    /// <summary>
    /// Interface for the store product repository class containing methods for interaction with the database
    /// </summary>
    public IStoreProductRepository StoreProducts { get; }

    /// <summary>
    /// Interface for the stock repository class containing methods for interaction with the database
    /// </summary>
    public IStockRepository Stocks { get; }

    /// <summary>
    /// Interface for the stock product repository class containing methods for interaction with the database
    /// </summary>
    public IStockProductRepository StockProducts { get; }

    /// <summary>
    /// Save changes
    /// </summary>
    /// <returns> Task object </returns>
    public Task SaveChangesAsync();
}
