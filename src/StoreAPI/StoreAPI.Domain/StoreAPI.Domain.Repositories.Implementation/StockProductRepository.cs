using Microsoft.EntityFrameworkCore;
using StoreAPI.Domain.Entities;
using StoreAPI.Domain.Repositories.Interfaces;

namespace StoreAPI.Domain.Repositories.Implementation;

/// <summary>
/// The stock product repository class containing methods for interaction with the database
/// </summary>
public class StockProductRepository : GenericRepository<StockProduct>, IStockProductRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="StockProductRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public StockProductRepository(Context context) : base(context)
    { }

    /// <inheritdoc/>
    public IQueryable<StockProduct> GetAll()
    {
        ThrowIfDisposed();
        return _db.AsNoTracking();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(StockProduct entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Update(entity));
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(StockProduct entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Remove(entity));
    }
}
