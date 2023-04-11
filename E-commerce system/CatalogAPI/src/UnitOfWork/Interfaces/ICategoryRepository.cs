using CatalogAPI.Models.DataBase;

namespace CatalogAPI.UnitOfWork.Interfaces;

/// <summary>
/// Interface for the category repository class containing methods for interaction with the database
/// </summary>
public interface ICategoryRepository : IGenericRepository<Category>
{
}
