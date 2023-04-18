using StoreAPI.DataBase;
using StoreAPI.DataBase.Entities;
using StoreAPI.UnitOfWork.Interfaces;

namespace StoreAPI.UnitOfWork;

/// <summary>
/// The cart store repository class containing methods for interaction with the database
/// </summary>
public class StoreRepository : GenericRepository<Store>, IStoreRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="StoreRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public StoreRepository(Context context) : base(context)
    { }
}
