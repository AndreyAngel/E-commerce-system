using Microsoft.EntityFrameworkCore;
using OrderAPI.DataBase;
using OrderAPI.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace OrderAPI.UnitOfWork;

/// <summary>
/// The generic repository class containing methods for interaction with the database
/// </summary>
/// <typeparam name="TEntity"> Entity type </typeparam>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Database context
    /// </summary>
    private readonly Context _context;

    private readonly DbSet<TEntity> _db;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Creates an instance of the <see cref="GenericRepository{TEntity}"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public GenericRepository(Context context)
    {
        _context = context;
        _db = context.Set<TEntity>();
    }

    ~GenericRepository() => Dispose(false);

    /// <inheritdoc/>
    public IQueryable<TEntity> GetAll()
    {
        ThrowIfDisposed();
        return _db.AsNoTracking();
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        ThrowIfDisposed();
        IQueryable<TEntity> query = _db;
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    /// <inheritdoc/>
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
                Console.WriteLine("Desposed");
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
