namespace OrderAPI.UnitOfWork.Interfaces;

/// <summary>
/// An interface for class that implements the unit of work pattern
/// and contains all entity repositories to create a single database context.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Interface for the cart repository class containing methods for interaction with the database
    /// </summary>
    public ICartRepository Carts { get; }

    /// <summary>
    /// Interface for the cart product repository class containing methods for interaction with the database
    /// </summary>
    public ICartProductRepository CartProducts { get; }

    /// <summary>
    /// Interface for the order repository class containing methods for interaction with the database
    /// </summary>
    public IOrderRepository Orders { get; }

    /// <summary>
    /// Save changes
    /// </summary>
    /// <returns> Task object </returns>
    public Task SaveChangesAsync();
}
