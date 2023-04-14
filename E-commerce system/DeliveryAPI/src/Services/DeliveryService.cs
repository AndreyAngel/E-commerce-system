using DeliveryAPI.DataBase;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;
using OrderAPI.Exceptions;

namespace DeliveryAPI.Services;

public class DeliveryService : IDeliveryService
{
    private readonly IUnitOfWork _unitOfWork;

    public DeliveryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Delivery> GetAll()
    {
        return _unitOfWork.Deliveries.GetAll();
    }

    public Delivery GetById(Guid Id)
    {
        var delivery = _unitOfWork.Deliveries.Include(x => x.Address, x => x.Courier)
                                             .FirstOrDefault(x => x.Id == Id);

        if (delivery == null)
        {
            throw new NotFoundException("Delivery with this Id wasn't founded", nameof(Id));
        }

        return delivery;
    }

    public async Task<Delivery> Create(Delivery delivery)
    {
        if (_unitOfWork.Deliveries.GetAll().FirstOrDefault(x => x.OrderId == delivery.OrderId) != null)
        {
            throw new ObjectNotUniqueException("Delivery with this order Id already exists");
        }

        await _unitOfWork.Deliveries.AddAsync(delivery);

        return delivery;
    }

    public void PickUpOrderFromWarehouse(Guid Id, Guid courierId)
    {
        var delivery = _unitOfWork.Deliveries.GetById(Id);

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
        var delivery = _unitOfWork.Deliveries.GetById(Id);

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
        var delivery = _unitOfWork.Deliveries.GetById(Id);

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
        var delivery = _unitOfWork.Deliveries.GetById(Id);

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
        throw new NotImplementedException();
    }
}
