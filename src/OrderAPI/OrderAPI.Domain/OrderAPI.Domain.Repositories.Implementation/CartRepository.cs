using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Repositories.Interfaces;

namespace OrderAPI.Domain.Repositories.Implementation;

/// <summary>
/// The cart repository class containing methods for interaction with the database
/// </summary>
public class CartRepository : GenericRepository<Cart>, ICartRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="CartRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public CartRepository(Context context) : base(context)
    { }
}
