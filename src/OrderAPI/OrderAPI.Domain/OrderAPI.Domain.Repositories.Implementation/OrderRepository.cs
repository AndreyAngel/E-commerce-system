using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Repositories.Interfaces;

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
}
