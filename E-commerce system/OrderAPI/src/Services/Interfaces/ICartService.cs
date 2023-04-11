using OrderAPI.Models;

namespace OrderAPI.Services.Interfaces;

/// <summary>
/// Interface for class providing the APIs for managing cart in a persistence store.
/// </summary>
public interface ICartService : IDisposable
{
    /// <summary>
    /// Get cart by Id
    /// </summary>
    /// <param name="id"> Cart id </param>
    /// <returns> Task object containing of the cart </returns>
    Task<CartDomainModel> GetById(Guid id);

    /// <summary>
    /// Create a new cart
    /// </summary>
    /// <param name="id"> New cart Id </param>
    /// <returns> Task object containing of created cart </returns>
    Task Create(Guid id);

    /// <summary>
    /// Compute total value of cart products
    /// </summary>
    /// <param name="id"> Cart Id </param>
    /// <returns> Task object containing of the cart </returns>
    Task<CartDomainModel> ComputeTotalValue(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns> Task object containing of the empty cart </returns>
    Task<CartDomainModel> Clear(Guid id);
}
