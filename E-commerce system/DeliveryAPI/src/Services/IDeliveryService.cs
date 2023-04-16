using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.Services;

public interface IDeliveryService : IDisposable
{
    IEnumerable<Delivery> GetAll();

    Delivery GetById(Guid Id);

    Task<Delivery> Create(Delivery delivery);

    void PickUpOrderFromWarehouse(Guid Id, Guid courierId);

    void Complete(Guid Id);

    void Cancel(Guid Id);

    void ReturnToWarehouse(Guid Id);

    Task<bool> ConfirmOrderId(Guid orderId);
}
