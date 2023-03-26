using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly Context _context;

    private bool _isDisposed = false;

    public IProductRepository Products { get; private set; }
    public ICategoryRepository Categories { get; private set; }
    public IBrandRepository Brands { get; private set; }

    public UnitOfWork(Context context)
    {
        _context = context;
        Products = new ProductRepository(context);
        Categories = new CategoryRepository(context);
        Brands = new BrandRepository(context);
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
