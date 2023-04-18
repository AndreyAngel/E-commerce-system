using StoreAPI.DataBase.Entities;

namespace StoreAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the stock repository class containing methods for interaction with the database
/// </summary>
public interface IStockRepository : IGenericRepository<Stock>
{
}
