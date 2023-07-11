using Microsoft.EntityFrameworkCore;
using StoreAPI.Domain.Entities;
using StoreAPI.Domain.Repositories.Interfaces;

namespace StoreAPI.Domain.Repositories.Implementation;

/// <summary>
/// The cart store repository class containing methods for interaction with the database
/// </summary>
public class StoreProductRepository : GenericRepository<StoreProduct>, IStoreProductRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="StoreProductRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public StoreProductRepository(Context context) : base(context)
    { }

    /// <inheritdoc/>
    public IQueryable<StoreProduct> GetAll()
    {
        ThrowIfDisposed();
        return _db.AsNoTracking();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(StoreProduct entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Update(entity));
    }
}
