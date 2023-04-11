using CatalogAPI.Models.DataBase;

namespace CatalogAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the product repository class containing methods for interaction with the database
/// </summary>
public interface IProductRepository : IGenericRepository<Product>
{
}
