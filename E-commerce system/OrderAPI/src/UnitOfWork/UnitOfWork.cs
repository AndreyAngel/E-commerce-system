using OrderAPI.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    public ICartRepository Carts { get; private set; }
    public ICartProductRepository CartProducts { get; private set; }
    public IOrderRepository Orders { get; private set; }

    public UnitOfWork(Context context)
    {
        _context = context;
        Carts = new CartRepository(context);
        CartProducts = new CartProductRepository(context);
        Orders = new OrderRepository(context);
    }

    ~UnitOfWork() => Dispose(false);

    public async Task SaveChangesAsync()
    {
        ThrowIfDisposed();
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
