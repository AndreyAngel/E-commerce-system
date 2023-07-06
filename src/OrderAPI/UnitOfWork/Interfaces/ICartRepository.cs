using OrderAPI.DataBase.Entities;

namespace OrderAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the cart repository class containing methods for interaction with the database
/// </summary>
public interface ICartRepository : IGenericRepository<Cart>
{
}
