using DeliveryAPI.DataBase;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;
using Infrastructure.DTO;
using Infrastructure;
using Infrastructure.Exceptions;
using OrderAPI.Exceptions;
using MassTransit;

namespace DeliveryAPI.Services;

public class DeliveryService : IDeliveryService
{
    /// <summary>
    /// An interface for class that implements the unit of work pattern
    /// and contains all entity repositories to create a single database context.
    /// </summary>
    private readonly IUnitOfWork _db;

    /// <summary>
    /// Configurations of application
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// <see cref="IBusControl"/>.
    /// </summary>
    private readonly IBusControl _bus;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    public DeliveryService(IUnitOfWork unitOfWork, IConfiguration configuration, IBusControl bus)
    {
        _db = unitOfWork;
        _configuration = configuration;
        _bus = bus;
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

    public async void Complete(Guid Id)
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
        await OrderIsReceived(delivery.OrderId);
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

    public async Task<bool> ConfirmOrderId(Guid orderId)
    {
        ThrowIfDisposed();

        var response = await RabbitMQClient.Request<ConfirmOrderIdDTORabbitMQ, ConfirmOrderIdDTORabbitMQ>
                        (_bus, new ConfirmOrderIdDTORabbitMQ() { OrderId = orderId },
                        new($"{_configuration["RabbitMQ:Host"]}/confirmOrderIdQueue"));

        if (response.OrderId != orderId)
        {
            throw new NotFoundException(response.Error);
        }

        return true;
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

    private async Task OrderIsReceived(Guid orderId)
    {
        ThrowIfDisposed();

        await RabbitMQClient.Request(_bus, new OrderIsReceivedDTORabbitMQ() { OrderId = orderId},
            new($"{_configuration["RabbitMQ:Host"]}/orderIsReceivedQueue"));
    }
}
