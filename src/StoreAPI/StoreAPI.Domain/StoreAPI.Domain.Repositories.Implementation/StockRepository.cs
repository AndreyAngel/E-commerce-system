using Microsoft.EntityFrameworkCore;
using StoreAPI.Domain.Entities;
using StoreAPI.Domain.Repositories.Interfaces;
using System.Linq.Expressions;

namespace StoreAPI.Domain.Repositories.Implementation;

/// <summary>
/// The stock repository class containing methods for interaction with the database
/// </summary>
public class StockRepository : GenericRepository<Stock>, IStockRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="StockRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public StockRepository(Context context) : base(context)
    { }

    /// <inheritdoc/>
    public IQueryable<Stock> Include(params Expression<Func<Stock, object>>[] includeProperties)
    {
        ThrowIfDisposed();
        IQueryable<Stock> query = _db;
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    /// <inheritdoc/>
    public async Task AddAsync(Stock entity)
    {
        ThrowIfDisposed();
        await _db.AddAsync(entity);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(Stock entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Remove(entity));
    }
}
