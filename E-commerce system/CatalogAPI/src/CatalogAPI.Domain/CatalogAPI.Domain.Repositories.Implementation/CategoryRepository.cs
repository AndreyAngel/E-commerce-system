using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CatalogAPI.Domain.Repositories.Implementation;

/// <summary>
/// The category repository class containing methods for interaction with the database
/// </summary>
public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="CategoryRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    /// <param name="memoryCache"> Represents a local in-memory cache whose values are not serialized </param>
    public CategoryRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
    { }
}
