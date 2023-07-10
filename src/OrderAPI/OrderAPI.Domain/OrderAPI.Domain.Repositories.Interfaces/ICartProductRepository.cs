using OrderAPI.Domain.Entities;

namespace OrderAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the cart product repository class containing methods for interaction with the database
/// </summary>
public interface ICartProductRepository : IGenericRepository<CartProduct>
{
    /// <summary>
    /// Get all entity objects
    /// </summary>
    /// <returns> Query of entity objects </returns>
    IQueryable<CartProduct> GetAll();

    /// <summary>
    /// Get entity by Id
    /// </summary>
    /// <param name="Id"> Object Id </param>
    /// <returns> One entity object </returns>
    CartProduct? GetById(Guid Id);

    /// <summary>
    /// Remove entity object
    /// </summary>
    /// <param name="entity"> Entity object </param>
    /// <returns> Task object </returns>
    Task RemoveAsync(CartProduct entity);
}
