using StoreAPI.Domain.Repositories.Implementation;
using StoreAPI.Domain.Repositories.Interfaces;

namespace StoreAPI.UnitOfWork;

/// <summary>
/// The class that implements the unit of work pattern
/// and contains all entity repositories to create a single database context.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Database context
    /// </summary>
    private readonly Context _context;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Interface for the store repository class containing methods for interaction with the database
    /// </summary>
    public IStoreRepository Stores { get; private set; }

    /// <summary>
    /// Interface for the store product repository class containing methods for interaction with the database
    /// </summary>
    public IStoreProductRepository StoreProducts { get; private set; }

    /// <summary>
    /// Interface for the stock repository class containing methods for interaction with the database
    /// </summary>
    public IStockRepository Stocks { get; private set; }

    /// <summary>
    /// Interface for the stock product repository class containing methods for interaction with the database
    /// </summary>
    public IStockProductRepository StockProducts { get; private set; }

    /// <summary>
    /// Creates an instance of the <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public UnitOfWork(Context context)
    {
        _context = context;
        Stores = new StoreRepository(context);
        StoreProducts = new StoreProductRepository(context);
        Stocks = new StockRepository(context);
        StockProducts = new StockProductRepository(context);
    }

    ~UnitOfWork() => Dispose(false);

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        ThrowIfDisposed();
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
                Stores.Dispose();
                StoreProducts.Dispose();
                Stocks.Dispose();
                StockProducts.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}
