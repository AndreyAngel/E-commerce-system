using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.UnitOfWork.Interfaces;
using CatalogAPI.DataBase;

namespace CatalogAPI.Services;

/// <summary>
/// Сlass providing the APIs for managing brand in a persistence store.
/// </summary>
public class BrandService: IBrandService
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _db;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Creates an instance of the <see cref="BrandService"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    public BrandService(IUnitOfWork unitOfWork)
    {
        _db = unitOfWork;
    }

    ~BrandService() => Dispose(false);

    /// <inheritdoc/>
    public List<Brand> GetAll()
    {
        ThrowIfDisposed();
        return _db.Brands.GetAll().ToList();
    }

    /// <inheritdoc/>
    public Brand GetById(Guid id)
    {
        ThrowIfDisposed();
        var res = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Id == id);

        if (res == null)
        {
            throw new NotFoundException("category with this Id was not founded!", nameof(id));
        }

        return res;
    }

    /// <inheritdoc/>
    public Brand GetByName(string name)
    {
        ThrowIfDisposed();
        var res = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Name == name);

        if (res == null)
        {
            throw new NotFoundException("category with this name was not founded!", nameof(name));
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<Brand> Create(Brand category)
    {
        ThrowIfDisposed();
        if (_db.Brands.GetAll().SingleOrDefault(x => x.Name == category.Name) != null)
        {
            throw new ObjectNotUniqueException("category with this name alredy exists!", nameof(category.Name));
        }

        await _db.Brands.AddAsync(category);

        return category;
    }

    /// <inheritdoc/>
    public async Task<Brand> Update(Brand category)
    {
        ThrowIfDisposed();
        var res = _db.Brands.GetById(category.Id);

        if (res == null)
        {
            throw new NotFoundException("category with this Id was not founded!", nameof(category.Id));
        }

        else if ((res.Name != category.Name) && _db.Brands.GetAll().SingleOrDefault(x => x.Name == category.Name) != null)
        {
            throw new ObjectNotUniqueException("category with this name already exists!", nameof(category.Name));
        }

        await _db.Brands.UpdateAsync(category);

        return category;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}
