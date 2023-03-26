namespace OrderAPI.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    public ICartRepository Carts { get; }

    public ICartProductRepository CartProducts { get; }

    public IOrderRepository Orders { get; }

    public Task SaveChangesAsync();

    public void Dispose();
}
