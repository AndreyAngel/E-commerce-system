using DeliveryAPI.Domain.Entities;
using DeliveryAPI.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DeliveryAPI.Domain.Repositories.Implementation;

/// <summary>
/// The delivery repository class containing methods for interaction with the database
/// </summary>
public class DeliveryRepository : GenericRepository<Delivery>, IDeliveryRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="DeliveryRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public DeliveryRepository(Context context) : base(context)
    { }

    /// <inheritdoc/>
    public IQueryable<Delivery> Include(params Expression<Func<Delivery, object>>[] includeProperties)
    {
        ThrowIfDisposed();
        IQueryable<Delivery> query = _db.AsNoTracking();
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }
}
