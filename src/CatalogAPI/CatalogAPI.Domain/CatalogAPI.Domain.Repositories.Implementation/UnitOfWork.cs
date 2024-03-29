﻿using CatalogAPI.Domain.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CatalogAPI.Domain.Repositories.Implementation;

/// <summary>
/// The class that implements the unit of work pattern
/// and contains all entity repositories to create a single database context.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Database context
    /// </summary>
    private readonly Context _context;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <inheritdoc/>
    public IProductRepository Products { get; private set; }

    /// <inheritdoc/>
    public ICategoryRepository Categories { get; private set; }

    /// <inheritdoc/>
    public IBrandRepository Brands { get; private set; }

    /// <summary>
    /// Creates an instance of the <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    /// <param name="memoryCache"> Represents a local in-memory cache whose values are not serialized </param>
    public UnitOfWork(Context context, IMemoryCache memoryCache)
    {
        _context = context;
        Products = new ProductRepository(context, memoryCache);
        Categories = new CategoryRepository(context, memoryCache);
        Brands = new BrandRepository(context, memoryCache);
    }

    ~UnitOfWork() => Dispose(false);

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        ThrowIfDisposed();
        await _context.SaveChangesAsync();
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
                _context.Dispose();
                Products.Dispose();
                Categories.Dispose();
                Brands.Dispose();
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
