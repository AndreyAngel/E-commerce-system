namespace OrderAPI.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    public IProductRepository Products { get; }

    public ICategoryRepository Categories { get; }

    public IBrandRepository Brands { get; }

    public Task SaveChangesAsync();

    public void Dispose();
}
