namespace CatalogAPI.UnitOfWork.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IProductRepository Products { get; }

    public ICategoryRepository Categories { get; }

    public IBrandRepository Brands { get; }

    public Task SaveChangesAsync();
}
