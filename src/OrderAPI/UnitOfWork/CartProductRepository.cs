using OrderAPI.DataBase;
using OrderAPI.DataBase.Entities;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

/// <summary>
/// The cart product repository class containing methods for interaction with the database
/// </summary>
public class CartProductRepository : GenericRepository<CartProduct>, ICartProductRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="CartProductRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public CartProductRepository(Context context) : base(context)
    { }
}
