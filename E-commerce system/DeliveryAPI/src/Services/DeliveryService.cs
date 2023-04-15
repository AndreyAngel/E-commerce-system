using DeliveryAPI.DataBase;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;
using OrderAPI.Exceptions;

namespace DeliveryAPI.Services;

public class DeliveryService : IDeliveryService
{
    private readonly IUnitOfWork _db;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    public DeliveryService(IUnitOfWork unitOfWork)
    {
        _db = unitOfWork;
    }

    ~DeliveryService() => Dispose(false);

    public IEnumerable<Delivery> GetAll()
    {
        ThrowIfDisposed();
        return _db.Deliveries.Include(x => x.Address, x => x.Courier);
    }

    public Delivery GetById(Guid Id)
    {
        ThrowIfDisposed();
        var delivery = _db.Deliveries.Include(x => x.Address, x => x.Courier)
                                             .FirstOrDefault(x => x.Id == Id);

        if (delivery == null)
        {
            throw new NotFoundException("Delivery with this Id wasn't founded", nameof(Id));
        }

        return delivery;
    }

    public async Task<Delivery> Create(Delivery delivery)
    {
        ThrowIfDisposed();

        if (_db.Deliveries.GetAll().FirstOrDefault(x => x.OrderId == delivery.OrderId) != null)
        {
            throw new ObjectNotUniqueException("Delivery with this order Id already exists");
        }

        delivery.Id = delivery.OrderId;
        await _db.Deliveries.AddAsync(delivery);

        return delivery;
    }

    public void PickUpOrderFromWarehouse(Guid Id, Guid courierId)
    {
        ThrowIfDisposed();
        var delivery = _db.Deliveries.GetById(Id);

        if (delivery == null)
        {
            throw new NotFoundException("Delivery with this Id wasn't founded", nameof(Id));
        }

        if (delivery.Status == DeliveryStatus.TheOrderReceivedByCustomer)
        {
            throw new DeliveryStatusException("Delivery has already recieved by customer");
        }

        delivery.CourierId = courierId;
        delivery.Status = DeliveryStatus.TheOrderReceivedByCourier;
    }

    public void Complete(Guid Id)
    {
        ThrowIfDisposed();
        var delivery = _db.Deliveries.GetById(Id);

        if (delivery == null)
        {
            throw new NotFoundException("Delivery with this Id wasn't founded", nameof(Id));
        }

        if (delivery.Status == DeliveryStatus.WaitingForTheCourier)
        {
            throw new DeliveryStatusException("Order is still waiting for the courier");
        }

        if (delivery.Status == DeliveryStatus.Canceled)
        {
            throw new DeliveryStatusException("Delivery has already canceled");
        }

        if (delivery.Status == DeliveryStatus.ReturnedToWarehouse)
        {
            throw new DeliveryStatusException("Order has already returned to warehouse");
        }

        delivery.Status = DeliveryStatus.TheOrderReceivedByCustomer;
    }

    public void Cancel(Guid Id)
    {
        ThrowIfDisposed();
        var delivery = _db.Deliveries.GetById(Id);

        if (delivery == null)
        {
            return;
        }

        if (delivery.Status == DeliveryStatus.TheOrderReceivedByCustomer)
        {
            return;
        }

        delivery.Status = DeliveryStatus.Canceled;
    }

    public void ReturnToWarehouse(Guid Id)
    {
        ThrowIfDisposed();
        var delivery = _db.Deliveries.GetById(Id);

        if (delivery == null)
        {
            throw new NotFoundException("Delivery with this Id wasn't founded", nameof(Id));
        }

        if (delivery.Status == DeliveryStatus.WaitingForTheCourier)
        {
            throw new DeliveryStatusException("Order is still waiting for the courier");
        }

        if (delivery.Status == DeliveryStatus.TheOrderReceivedByCustomer)
        {
            throw new DeliveryStatusException("Order has already recieved by customer");
        }

        delivery.Status = DeliveryStatus.ReturnedToWarehouse;
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
