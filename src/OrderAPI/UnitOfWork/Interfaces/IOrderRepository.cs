using OrderAPI.DataBase.Entities;

namespace OrderAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the order repository class containing methods for interaction with the database
/// </summary>
public interface IOrderRepository : IGenericRepository<Order>
{
}
