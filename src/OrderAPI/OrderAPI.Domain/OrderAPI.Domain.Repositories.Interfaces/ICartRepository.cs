using OrderAPI.Domain.Entities;

namespace OrderAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the cart repository class containing methods for interaction with the database
/// </summary>
public interface ICartRepository : IGenericRepository<Cart>
{
}
