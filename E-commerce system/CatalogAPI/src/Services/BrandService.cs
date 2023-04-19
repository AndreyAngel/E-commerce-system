using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.UnitOfWork.Interfaces;
using CatalogAPI.DataBase;
using Microsoft.Extensions.Caching.Memory;

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
    /// Represents a local in-memory cache whose values are not serialized
    /// </summary>
    private readonly IMemoryCache _cache;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Creates an instance of the <see cref="BrandService"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    /// <param name="memoryCache"> Represents a local in-memory cache whose values are not serialized </param>
    public BrandService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _db = unitOfWork;
        _cache = memoryCache;
    }

    ~BrandService() => Dispose(false);

    /// <inheritdoc/>
    public List<Brand> GetAll()
    {
        ThrowIfDisposed();

        if (!_cache.TryGetValue(typeof(List<Brand>), out List<Brand>? brands))
        {
            brands = _db.Brands.GetAll().ToList();
            _cache.Set(typeof(List<Brand>), brands,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }

        return brands;
    }

    /// <inheritdoc/>
    public Brand GetById(Guid id)
    {
        ThrowIfDisposed();

        if (!_cache.TryGetValue(id, out Brand? brand))
        {
            brand = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Id == id);

            if (brand == null)
            {
                throw new NotFoundException("Brand with this Id was not founded!", nameof(id));
            }

            _cache.Set(id, brand,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }

        return brand;
    }

    /// <inheritdoc/>
    public Brand GetByName(string name)
    {
        ThrowIfDisposed();

        if (!_cache.TryGetValue(name, out Brand? brand))
        {
            brand = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Name == name);

            if (brand == null)
            {
                throw new NotFoundException("Brand with this name was not founded!", nameof(name));
            }

            _cache.Set(name, brand,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }

        return brand;
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
