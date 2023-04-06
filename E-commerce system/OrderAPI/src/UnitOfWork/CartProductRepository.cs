using OrderAPI.DataBase;
using OrderAPI.DataBase.Entities;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class CartProductRepository : GenericRepository<CartProduct>, ICartProductRepository
{
    public CartProductRepository(Context context) : base(context)
    { }
}
