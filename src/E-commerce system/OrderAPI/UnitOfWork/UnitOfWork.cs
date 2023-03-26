using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly Context _context;

    private bool _isDisposed = false;

    public ICartRepository Carts { get; private set; }
    public ICartProductRepository CartProducts { get; private set; }
    public IOrderRepository Orders { get; private set; }

    public UnitOfWork(Context context)
    {
        _context = context;
        Carts = new CartRepository(context);
        CartProducts = new CartProductRepository(context);
        Orders = new OrderRepository(context);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        // Освобождаем все ресурсы
        Dispose(true);

        // Подавляем финализацию
        GC.SuppressFinalize(this);
    }

    public virtual void Dispose(bool disposing)
    {
        if (!_isDisposed && disposing)
        {
            // Освобождаем управляемые ресурсы
        }

        // Освобождаем неуправляемые ресурсы
        _context.Dispose();
        _isDisposed = true;
    }
}
