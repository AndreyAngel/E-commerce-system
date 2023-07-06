using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the product repository class containing methods for interaction with the database
/// </summary>
public interface IProductRepository : IGenericRepository<Product>
{
}
