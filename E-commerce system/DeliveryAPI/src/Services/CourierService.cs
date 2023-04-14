using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;

namespace DeliveryAPI.Services;

public class CourierService : ICourierService
{
    public readonly IUnitOfWork _db;

    private bool _disposed;

    public CourierService(IUnitOfWork db)
    {
        _db = db;
    }

    ~CourierService() => Dispose(false);

    public async Task Create(Courier courier)
    {
        await _db.Couriers.AddAsync(courier);
        await _db.SaveChangesAsync();
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
