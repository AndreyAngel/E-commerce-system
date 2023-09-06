using CatalogAPI.Domain.Entities.Abstractions;
using CatalogAPI.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace CatalogAPI.Domain.Repositories.Implementation;

/// <summary>
/// The generic repository class containing methods for interaction with the database
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
{
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
    /// Database context
    /// </summary>
    protected readonly Context _context;

    protected readonly DbSet<TEntity> _brandTable;

    /// <summary>
    /// Creates an instance of the <see cref="GenericRepository{TEntity}"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    /// <param name="memoryCache"> Represents a local in-memory cache whose values are not serialized </param>
    public GenericRepository(Context context, IMemoryCache memoryCache)
    {
        _cache = memoryCache;
        _context = context;
        _brandTable = context.Set<TEntity>();
    }

    ~GenericRepository() => Dispose(false);

    /// <inheritdoc/>
    public IQueryable<TEntity> GetAll()
    {
        ThrowIfDisposed();

        if (!_cache.TryGetValue(typeof(IQueryable<TEntity>), out IQueryable<TEntity>? entities))
        {
            entities = _brandTable.AsNoTracking();
            _cache.Set(typeof(IQueryable<TEntity>), entities,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }

        return entities;
    }

    /// <inheritdoc/>
    public List<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        ThrowIfDisposed();

        if (!_cache.TryGetValue(typeof(List<TEntity>) + "Include", out List<TEntity>? entities))
        {
            IQueryable<TEntity> query = _brandTable.AsNoTracking();
            entities = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).ToList();

            _cache.Set(typeof(List<TEntity>) + "Include", entities,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }

        return entities;
    }

    /// <inheritdoc/>
    public TEntity? GetById(Guid Id)
    {
        ThrowIfDisposed();

        if (!_cache.TryGetValue(Id, out TEntity? entity))
        {
            entity = _brandTable.Find(Id);

            if (entity == null)
            {
                return null;
            }

            //_context.Entry(entity).State = EntityState.Detached;

            _cache.Set(Id, entity,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }

        return entity;
    }

    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity)
    {
        ThrowIfDisposed();
        await _brandTable.AddAsync(entity);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(TEntity entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _brandTable.Update(entity));
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
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
