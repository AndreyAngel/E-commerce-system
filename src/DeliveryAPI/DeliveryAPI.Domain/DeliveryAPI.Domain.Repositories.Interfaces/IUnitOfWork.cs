namespace DeliveryAPI.Domain.Repositories.Interfaces;

/// <summary>
/// An interface for class that implements the unit of work pattern
/// and contains all entity repositories to create a single database context.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Interface for the delivery repository class containing methods for interaction with the database
    /// </summary>
    public IDeliveryRepository Deliveries { get; }

    /// <summary>
    /// Interface for the courier repository class containing methods for interaction with the database
    /// </summary>
    public ICourierRepository Couriers { get; }

    /// <summary>
    /// Save changes
    /// </summary>
    /// <returns> Task object </returns>
    public Task SaveChangesAsync();
}
