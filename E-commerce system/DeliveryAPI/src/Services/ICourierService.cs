using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.Services;

/// <summary>
/// Interface for class providing the APIs for managing courier data in a persistence store.
/// </summary>
public interface ICourierService : IDisposable
{
    /// <summary>
    /// Get information about all couriers
    /// </summary>
    /// <returns> Information about all couriers </returns>
    IEnumerable<Courier> GetAll();

    /// <summary>
    /// Get information about single courier by Id
    /// </summary>
    /// <param name="Id"> Courier Id </param>
    /// <returns> Information about single courier </returns>
    Courier GetById(Guid Id);

    /// <summary>
    /// Create of the new courier
    /// </summary>
    /// <param name="courier"> Courier object </param>
    /// <returns> Task object </returns>
    Task Create(Courier courier);
}
