using DeliveryAPI.DataBase;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;

namespace DeliveryAPI.Services;

/// <summary>
/// Class providing the APIs for managing courier data in a persistence store.
/// </summary>
public class CourierService : ICourierService
{
    /// <summary>
    /// A repository group interface providing a common data context
    /// </summary>
    public readonly IUnitOfWork _db;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// Creates an instance of the <see cref="CourierService"/>.
    /// </summary>
    /// <param name="db"> A repository group interface providing a common data context </param>
    public CourierService(IUnitOfWork db)
    {
        _db = db;
    }

    ~CourierService() => Dispose(false);

    /// <inheritdoc/>
    public IEnumerable<Courier> GetAll()
    {
        ThrowIfDisposed();
        return _db.Couriers.GetAll();
    }

    /// <inheritdoc/>
    public Courier GetById(Guid Id)
    {
        ThrowIfDisposed();

        var result = _db.Couriers.GetById(Id);

        if (result == null)
        {
            throw new NotFoundException("Courier with this Id wasn't founded", nameof(Id));
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task Create(Courier courier)
    {
        ThrowIfDisposed();
        await _db.Couriers.AddAsync(courier);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
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
                _db.Dispose();
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
