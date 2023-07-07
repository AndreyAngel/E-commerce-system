using StoreAPI.Domain.Entities;
using StoreAPI.Domain.Repositories.Interfaces;

namespace StoreAPI.Domain.Repositories.Implementation;

/// <summary>
/// The stock repository class containing methods for interaction with the database
/// </summary>
public class StockRepository : GenericRepository<Stock>, IStockRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="StockRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public StockRepository(Context context) : base(context)
    { }
}
