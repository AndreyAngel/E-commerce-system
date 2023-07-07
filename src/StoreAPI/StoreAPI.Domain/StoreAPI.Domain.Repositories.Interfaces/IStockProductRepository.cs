using StoreAPI.Domain.Entities;

namespace StoreAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the stock product repository class containing methods for interaction with the database
/// </summary>
public interface IStockProductRepository : IGenericRepository<StockProduct>
{
}
