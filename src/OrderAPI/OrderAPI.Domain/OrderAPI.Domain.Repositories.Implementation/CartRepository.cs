using Microsoft.EntityFrameworkCore;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Repositories.Interfaces;
using System.Linq.Expressions;

namespace OrderAPI.Domain.Repositories.Implementation;

/// <summary>
/// The cart repository class containing methods for interaction with the database
/// </summary>
public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="CartRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public CartRepository(Context context) : base(context)
    { }

    /// <inheritdoc/>
    public IQueryable<Cart> Include(params Expression<Func<Cart, object>>[] includeProperties)
    {
        ThrowIfDisposed();
        IQueryable<Cart> query = _db;
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }
}
