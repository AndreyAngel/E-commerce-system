using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the courier repository class containing methods for interaction with the database
/// </summary>
public interface ICourierRepository : IGenericRepositoty<Courier>
{
}
