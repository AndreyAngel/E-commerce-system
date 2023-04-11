using OrderAPI.DataBase;
using OrderAPI.DataBase.Entities;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

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
