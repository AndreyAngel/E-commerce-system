using OrderAPI.DataBase;
using OrderAPI.DataBase.Entities;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(Context context) : base(context)
    { }
}
