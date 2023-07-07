using StoreAPI.Domain.Entities;

namespace StoreAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the store product repository class containing methods for interaction with the database
/// </summary>
public interface IStoreProductRepository : IGenericRepository<StoreProduct>
{
}
