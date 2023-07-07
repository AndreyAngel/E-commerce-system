using StoreAPI.Domain.Entities;

namespace StoreAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the stock repository class containing methods for interaction with the database
/// </summary>
public interface IStockRepository : IGenericRepository<Stock>
{
}
