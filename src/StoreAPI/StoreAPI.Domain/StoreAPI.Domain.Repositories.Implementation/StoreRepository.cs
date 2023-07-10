using Microsoft.EntityFrameworkCore;
using StoreAPI.Domain.Entities;
using StoreAPI.Domain.Repositories.Interfaces;

namespace StoreAPI.Domain.Repositories.Implementation;

/// <summary>
/// The cart store repository class containing methods for interaction with the database
/// </summary>
public class StoreRepository : GenericRepository<Store>, IStoreRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="StoreRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public StoreRepository(Context context) : base(context)
    { }

    /// <inheritdoc/>
    public IQueryable<Store> GetAll()
    {
        ThrowIfDisposed();
        return _db.AsNoTracking();
    }

    /// <inheritdoc/>
    public Store? GetById(Guid Id)
    {
        ThrowIfDisposed();
        return _db.Find(Id);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Store entity)
    {
        ThrowIfDisposed();
        await _db.AddAsync(entity);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Store entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Update(entity));
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(Store entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Remove(entity));
    }
}
