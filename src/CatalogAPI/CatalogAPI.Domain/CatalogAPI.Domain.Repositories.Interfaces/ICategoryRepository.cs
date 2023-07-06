using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Repositories.Interfaces;

/// <summary>
/// Interface for the category repository class containing methods for interaction with the database
/// </summary>
public interface ICategoryRepository : IGenericRepository<Category>
{
}
