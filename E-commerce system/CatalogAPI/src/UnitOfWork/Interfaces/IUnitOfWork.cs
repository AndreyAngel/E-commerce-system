namespace CatalogAPI.UnitOfWork.Interfaces;

/// <summary>
/// An interface for class that implements the unit of work pattern
/// and contains all entity repositories to create a single database context.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Interface for the product repository class containing methods for interaction with the database
    /// </summary>
    public IProductRepository Products { get; }

    /// <summary>
    /// Interface for the category repository class containing methods for interaction with the database
    /// </summary>
    public ICategoryRepository Categories { get; }

    /// <summary>
    /// Interface for the brand repository class containing methods for interaction with the database
    /// </summary>
    public IBrandRepository Brands { get; }

    /// <summary>
    /// Save changes
    /// </summary>
    /// <returns> Task object </returns>
    public Task SaveChangesAsync();
}
