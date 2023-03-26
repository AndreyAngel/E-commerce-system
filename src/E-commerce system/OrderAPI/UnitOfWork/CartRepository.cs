using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    public CartRepository(Context context) : base(context)
    { }
}
