using StoreAPI.Domain.Entities;
using StoreAPI.Domain.Repositories.Interfaces;

namespace StoreAPI.Domain.Repositories.Implementation;

/// <summary>
/// The stock product repository class containing methods for interaction with the database
/// </summary>
public class StockProductRepository : GenericRepository<StockProduct>, IStockProductRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="StockProductRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public StockProductRepository(Context context) : base(context)
    { }
}
