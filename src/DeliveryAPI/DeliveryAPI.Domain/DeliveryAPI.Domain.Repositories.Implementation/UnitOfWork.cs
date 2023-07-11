using DeliveryAPI.Domain.Repositories.Interfaces;

namespace DeliveryAPI.Domain.Repositories.Implementation;

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

    /// <inheritdoc/>
    public IDeliveryRepository Deliveries { get; private set; }

    /// <inheritdoc/>
    public ICourierRepository Couriers { get; private set; }

    /// <summary>
    /// Creates an instance of the <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public UnitOfWork(Context context)
    {
        _context = context;
        Deliveries = new DeliveryRepository(context);
        Couriers = new CourierRepository(context);
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
                Deliveries.Dispose();
                Couriers.Dispose();
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
