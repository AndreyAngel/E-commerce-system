using CatalogAPI.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.UnitOfWork;

/// <summary>
/// The brand repository class containing methods for interaction with the database
/// </summary>
public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="BrandRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public BrandRepository(Context context) : base(context)
    { }
}
