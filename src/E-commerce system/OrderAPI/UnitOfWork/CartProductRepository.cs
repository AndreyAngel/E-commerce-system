using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class CartProductRepository : GenericRepository<CartProduct>, ICartProductRepository
{
    public CartProductRepository(Context context) : base(context)
    { }
}
