using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.Models.DTO;

namespace DeliveryAPI.Services;

public interface IDeliveryService : IDisposable
{
    IEnumerable<Delivery> GetAll();

    Delivery GetById(Guid Id);

    IEnumerable<Delivery> GetByFilter(DeliveryFilterDTORequest filters);

    Task<Delivery> Create(Delivery delivery);

    void PickUpOrderFromWarehouse(Guid Id, Guid courierId);

    void Complete(Guid Id);

    void Cancel(Guid Id);

    void ReturnToWarehouse(Guid Id);

    Task<bool> ConfirmOrderId(Guid orderId);
}
