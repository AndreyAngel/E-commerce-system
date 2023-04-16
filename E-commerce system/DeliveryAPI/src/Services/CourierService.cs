using DeliveryAPI.DataBase;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;

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

    public IEnumerable<Courier> GetAll()
    {
        return _db.Couriers.GetAll();
    }

    public Courier GetById(Guid Id)
    {
        var result = _db.Couriers.Include(x => x.Deliveries
                           .Where(x => x.Status == DeliveryStatus.TheOrderReceivedByCourier))
                           .SingleOrDefault(x => x.Id == Id);

        if (result == null)
        {
            throw new NotFoundException("Courier with this Id wasn't founded", nameof(Id));
        }

        return result;
    }

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
