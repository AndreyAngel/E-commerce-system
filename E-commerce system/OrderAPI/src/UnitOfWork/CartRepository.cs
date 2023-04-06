using OrderAPI.DataBase;
using OrderAPI.DataBase.Entities;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    public CartRepository(Context context) : base(context)
    { }
}
