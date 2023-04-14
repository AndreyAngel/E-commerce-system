using CatalogAPI.DataBase;
using CatalogAPI.DataBase.Entities;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.UnitOfWork;

/// <summary>
/// The product repository class containing methods for interaction with the database
/// </summary>
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="ProductRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public ProductRepository(Context context) : base(context)
    { }
}
