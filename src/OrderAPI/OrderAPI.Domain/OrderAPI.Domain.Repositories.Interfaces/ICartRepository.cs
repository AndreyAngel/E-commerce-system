using OrderAPI.Domain.Entities;
using System.Linq.Expressions;

namespace OrderAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the cart repository class containing methods for interaction with the database
/// </summary>
public interface ICartRepository : IGenericRepository<Cart>
{
    /// <summary>
    /// Include data from another database table
    /// </summary>
    /// <param name="includeProperties"> Include properties </param>
    /// <returns> A new query with the released data included </returns>
    IQueryable<Cart> Include(params Expression<Func<Cart, object>>[] includeProperties);
}