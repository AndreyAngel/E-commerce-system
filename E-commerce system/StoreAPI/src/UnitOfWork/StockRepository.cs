using StoreAPI.DataBase;
using StoreAPI.DataBase.Entities;
using StoreAPI.UnitOfWork.Interfaces;

namespace StoreAPI.UnitOfWork;

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
