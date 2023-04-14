using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the delivery repository class containing methods for interaction with the database
/// </summary>
public interface IDeliveryRepository : IGenericRepositoty<Delivery>
{
}
