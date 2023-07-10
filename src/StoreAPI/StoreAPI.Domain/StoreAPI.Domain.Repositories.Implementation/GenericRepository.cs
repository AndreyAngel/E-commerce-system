using Microsoft.EntityFrameworkCore;
using StoreAPI.Domain.Repositories.Interfaces;
using System.Linq.Expressions;

namespace StoreAPI.Domain.Repositories.Implementation;

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

    protected readonly DbSet<TEntity> _db;

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
