using Microsoft.EntityFrameworkCore;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Repositories.Interfaces;
using System.Linq.Expressions;

namespace OrderAPI.Domain.Repositories.Implementation;

/// <summary>
/// The cart order repository class containing methods for interaction with the database
/// </summary>
public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="OrderRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public OrderRepository(Context context) : base(context)
    { }

    /// <inheritdoc/>
    public IQueryable<Order> GetAll()
    {
        ThrowIfDisposed();
        return _db.AsNoTracking();
    }

    /// <inheritdoc/>
    public Order? GetById(Guid Id)
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
    public IQueryable<Order> Include(params Expression<Func<Order, object>>[] includeProperties)
    {
        ThrowIfDisposed();
        IQueryable<Order> query = _db;
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }
}
