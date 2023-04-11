using OrderAPI.DataBase.Entities;

namespace OrderAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the cart product repository class containing methods for interaction with the database
/// </summary>
public interface ICartProductRepository : IGenericRepository<CartProduct>
{
}
