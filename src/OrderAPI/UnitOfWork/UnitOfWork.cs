using OrderAPI.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

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
    /// Interface for the cart repository class containing methods for interaction with the database
    /// </summary>
    public ICartRepository Carts { get; private set; }

    /// <summary>
    /// Interface for the cart product repository class containing methods for interaction with the database
    /// </summary>
    public ICartProductRepository CartProducts { get; private set; }

    /// <summary>
    /// Interface for the order repository class containing methods for interaction with the database
    /// </summary>
    public IOrderRepository Orders { get; private set; }

    /// <summary>
    /// Creates an instance of the <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public UnitOfWork(Context context)
    {
        _context = context;
        Carts = new CartRepository(context);
        CartProducts = new CartProductRepository(context);
        Orders = new OrderRepository(context);
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
                Carts.Dispose();
                CartProducts.Dispose();
                Orders.Dispose();
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
