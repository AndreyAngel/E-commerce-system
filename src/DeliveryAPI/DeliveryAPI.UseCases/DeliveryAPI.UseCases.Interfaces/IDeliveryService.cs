using DeliveryAPI.Contracts.DTO;
using DeliveryAPI.Domain.Entities;
using Infrastructure.Exceptions;
using DeliveryAPI.UseCases.Interfaces.Exceptions;

namespace DeliveryAPI.UseCases.Interfaces;

/// <summary>
/// Interface for class providing the APIs for managing delivery in a persistence store.
/// </summary>
public interface IDeliveryService : IDisposable
{
    /// <summary>
    /// Get all deliveries
    /// </summary>
    /// <returns> All deliveries </returns>
    IEnumerable<Delivery> GetAll();

    /// <summary>
    /// Get delivery by Id
    /// </summary>
    /// <param name="Id"> Delivery Id </param>
    /// <returns> <see cref="Delivery"/> </returns>
    /// <exception cref="NotFoundException"> Delivery with this Id wasn't founded </exception>
    Delivery GetById(Guid Id);

    /// <summary>
    /// Get deliveries by filters
    /// </summary>
    /// <param name="filters"> Filters </param>
    /// <returns> Deliveries </returns>
    IEnumerable<Delivery> GetByFilter(DeliveryFilterDTORequest filters);

    /// <summary>
    /// Create a new delivery
    /// </summary>
    /// <param name="delivery"> Delivery object </param>
    /// <returns> Task object containing of the <see cref="Delivery"/> </returns>
    /// <exception cref="ObjectNotUniqueException"> Delivery with this order Id already exists </exception>
    Task<Delivery> Create(Delivery delivery);

    /// <summary>
    /// Pick up of the order from warehouse by courier
    /// </summary>
    /// <param name="orderId"> Order Id </param>
    /// <param name="courierId"> Courier Id </param>
    /// <exception cref="NotFoundException"> Delivery with this order Id wasn't founded </exception>
    /// <exception cref="DeliveryStatusException"> Delivery has already recieved by customer </exception>
    void PickUpOrderFromWarehouse(Guid orderId, Guid courierId);

    /// <summary>
    /// Complete delivery
    /// </summary>
    /// <param name="Id"> Delivery Id </param>
    /// <exception cref="NotFoundException"> Delivery with this order Id wasn't founded </exception>
    /// <exception cref="DeliveryStatusException"></exception>
    void Complete(Guid Id);

    /// <summary>
    /// Cansel delivery
    /// </summary>
    /// <param name="Id"> Delivery Id </param>
    /// <exception cref="NotFoundException"> Delivery with this order Id wasn't founded </exception>
    /// <exception cref="DeliveryStatusException"> The order already received by customer </exception>
    void Cancel(Guid Id);

    /// <summary>
    /// Resturn of the order to warehouse bu courier
    /// </summary>
    /// <param name="orderId"> Order Id </param>
    /// <exception cref="NotFoundException"> Delivery with this order Id wasn't founded </exception>
    /// <exception cref="DeliveryStatusException"></exception>
    void ReturnToWarehouse(Guid orderId);
}
