using OrderAPI.DataBase;
using OrderAPI.DataBase.Entities;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

/// <summary>
/// The cart cart repository class containing methods for interaction with the database
/// </summary>
public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="CartProductRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public CartRepository(Context context) : base(context)
    { }
}
