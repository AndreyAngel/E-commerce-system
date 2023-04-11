using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.UnitOfWork;

/// <summary>
/// The category repository class containing methods for interaction with the database
/// </summary>
public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="CategoryRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public CategoryRepository(Context context) : base(context)
    { }
}
