using CatalogAPI.DataBase;
using CatalogAPI.DataBase.Entities;
using CatalogAPI.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace CatalogAPI.UnitOfWork;

/// <summary>
/// The generic repository class containing methods for interaction with the database
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
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

    protected readonly DbSet<TEntity> _db;

    /// <summary>
    /// Creates an instance of the <see cref="GenericRepository{TEntity}"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    /// <param name="memoryCache"> Represents a local in-memory cache whose values are not serialized </param>
    public GenericRepository(Context context, IMemoryCache memoryCache)
    {
        _cache = memoryCache;
        _context = context;
        _db = context.Set<TEntity>();
    }

    ~GenericRepository() => Dispose(false);

    /// <inheritdoc/>
    public List<TEntity> GetAll()
    {
        ThrowIfDisposed();

        if (!_cache.TryGetValue(typeof(List<TEntity>), out List<TEntity>? entities))
        {
            entities = _db.AsNoTracking().ToList();
            _cache.Set(typeof(List<TEntity>), entities,
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
            IQueryable<TEntity> query = _db.AsNoTracking();
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
            entity = _db.Find(Id);

            if (entity == null)
            {
                return null;
            }

            _context.Entry(entity).State = EntityState.Detached;

            _cache.Set(Id, entity,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }
            
        return entity;
    }

    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity)
    {
        ThrowIfDisposed();
        await _db.AddAsync(entity);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(TEntity entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Update(entity));
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(TEntity entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Remove(entity));
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
