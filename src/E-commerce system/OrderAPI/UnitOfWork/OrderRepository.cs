using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(Context context) : base(context)
    { }
}
