using StoreAPI.DataBase;
using StoreAPI.DataBase.Entities;
using StoreAPI.UnitOfWork.Interfaces;

namespace StoreAPI.UnitOfWork;

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
