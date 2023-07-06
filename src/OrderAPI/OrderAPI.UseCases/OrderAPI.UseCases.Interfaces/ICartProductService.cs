using OrderAPI.Domain.Models;

namespace OrderAPI.UseCases.Interfaces;

/// <summary>
/// Interface for class providing the APIs for managing cart product in a persistence store.
/// </summary>
public interface ICartProductService : IDisposable
{
    /// <summary>
    /// Create a new cart product
    /// </summary>
    /// <param name="cartProduct"> New cart product </param>
    /// <returns> Task object containing of created cart product </returns>
    Task<CartProductDomainModel> Create(CartProductDomainModel cartProduct);

    /// <summary>
    /// Update cart product
    /// </summary>
    /// <param name="cartProduct"> Cart product </param>
    /// <returns> Task object containing of updated cart product </returns>
    Task<CartProductDomainModel> Update(CartProductDomainModel cartProduct);

    /// <summary>
    /// Delete cart product
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Task object </returns>
    Task Delete(Guid id);
}
