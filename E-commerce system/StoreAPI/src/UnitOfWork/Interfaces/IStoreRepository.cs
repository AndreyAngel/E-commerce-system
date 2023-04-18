using StoreAPI.DataBase.Entities;

namespace StoreAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the store repository class containing methods for interaction with the database
/// </summary>
public interface IStoreRepository : IGenericRepository<Store>
{
}
