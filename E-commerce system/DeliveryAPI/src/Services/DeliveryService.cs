using DeliveryAPI.DataBase;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;
using Infrastructure.DTO;
using Infrastructure;
using Infrastructure.Exceptions;
using OrderAPI.Exceptions;
using MassTransit;
using DeliveryAPI.Models.DTO;

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

    /// <summary>
    /// Creates an instance of the <see cref="DeliveryService"/>.
    /// </summary>
    /// <param name="unitOfWork"> An interface for class that implements the unit of work pattern
    /// and contains all entity repositories to create a single database context. </param>
    /// <param name="configuration"> Configurations of application </param>
    /// <param name="bus"> <see cref="IBusControl"/>. </param>
    public DeliveryService(IUnitOfWork unitOfWork, IConfiguration configuration, IBusControl bus)
    {
        _db = unitOfWork;
        _configuration = configuration;
        _bus = bus;
    }

    ~DeliveryService() => Dispose(false);

    /// <inheritdoc/>
    public IEnumerable<Delivery> GetAll()
    {
        ThrowIfDisposed();
        return _db.Deliveries.Include(x => x.Address, x => x.Courier);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public IEnumerable<Delivery> GetByFilter(DeliveryFilterDTORequest filters)
    {
        ThrowIfDisposed();
        var deliveries = _db.Deliveries.Include(x => x.Address, x => x.Courier);

        if (filters.Status != null)
        {
            deliveries = deliveries.Where(x => x.Status == filters.Status);
        }

        if (filters.CourierId != Guid.Empty)
        {
            deliveries = deliveries.Where(x => x.CourierId == filters.CourierId);
        }

        if (filters.Address != null)
        {
            if (filters.Address.City != null)
            {
                deliveries = deliveries.Where(x => x.Address.City == filters.Address.City);
            }

            if (filters.Address.Street != null)
            {
                deliveries = deliveries.Where(x => x.Address.Street == filters.Address.Street);
            }

            if (filters.Address.NumberOfHome != null)
            {
                deliveries = deliveries.Where(x => x.Address.NumberOfHome == filters.Address.NumberOfHome);
            }

            if (filters.Address.ApartmentNumber != null)
            {
                deliveries = deliveries.Where(x => x.Address.ApartmentNumber == filters.Address.ApartmentNumber);
            }

            if (filters.Address.PostalCode != null)
            {
                deliveries = deliveries.Where(x => x.Address.PostalCode == filters.Address.PostalCode);
            }
        }

        return deliveries;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void Cancel(Guid Id)
    {
        ThrowIfDisposed();
        var delivery = _db.Deliveries.GetById(Id);

        if (delivery == null)
        {
            throw new NotFoundException("Delivery with this Id wasn't founded", nameof(Id));
        }

        if (delivery.Status == DeliveryStatus.TheOrderReceivedByCustomer)
        {
            throw new DeliveryStatusException("The order already received by customer");
        }

        delivery.Status = DeliveryStatus.Canceled;
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Send of message about received of the order by customer
    /// </summary>
    /// <param name="orderId"> Order Id </param>
    /// <returns> Task object </returns>
    private async Task OrderIsReceived(Guid orderId)
    {
        ThrowIfDisposed();

        await RabbitMQClient.Request(_bus, new OrderIsReceivedDTORabbitMQ() { OrderId = orderId},
            new($"{_configuration["RabbitMQ:Host"]}/orderIsReceivedQueue"));
    }
}
