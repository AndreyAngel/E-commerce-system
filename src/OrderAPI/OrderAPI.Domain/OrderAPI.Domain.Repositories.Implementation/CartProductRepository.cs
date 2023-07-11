using Microsoft.EntityFrameworkCore;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Repositories.Interfaces;

namespace OrderAPI.Domain.Repositories.Implementation;

/// <summary>
/// The cart product repository class containing methods for interaction with the database
/// </summary>
public class CartProductRepository : GenericRepository<CartProduct>, ICartProductRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="CartProductRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public CartProductRepository(Context context) : base(context)
    { }

    /// <inheritdoc/>
    public IQueryable<CartProduct> GetAll()
    {
        ThrowIfDisposed();
        return _db.AsNoTracking();
    }

    /// <inheritdoc/>
    public CartProduct? GetById(Guid Id)
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
    public async Task RemoveAsync(CartProduct entity)
    {
        ThrowIfDisposed();
        await Task.Run(() => _db.Remove(entity));
    }
}
