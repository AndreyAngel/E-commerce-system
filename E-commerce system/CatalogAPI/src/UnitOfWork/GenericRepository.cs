using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogAPI.UnitOfWork;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
{
    private protected readonly Context _context;

    private readonly DbSet<TEntity> _db;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    public GenericRepository(Context context)
    {
        _context = context;
        _db = context.Set<TEntity>();
    }

    ~GenericRepository() => Dispose(false);

    public IEnumerable<TEntity> GetAll()
    {
        ThrowIfDisposed();
        return _db.AsNoTracking();
    }

    public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        ThrowIfDisposed();
        IQueryable<TEntity> query = _db.AsNoTracking();
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    public TEntity? GetById(Guid Id)
    {
        ThrowIfDisposed();
        var entity = _db.Find(Id);

        if (entity == null)
        {
            return null;
        }

        _context.Entry(entity).State = EntityState.Detached;

        return entity;
    }

    public async Task AddAsync(TEntity entity)
    {
        ThrowIfDisposed();
        await _db.AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Update(entity));
    }

    public async Task RemoveAsync(TEntity entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Remove(entity));
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
