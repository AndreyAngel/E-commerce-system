using CatalogAPI.Models.DataBase;

namespace CatalogAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the brand repository class containing methods for interaction with the database
/// </summary>
public interface IBrandRepository : IGenericRepository<Brand>
{
}
