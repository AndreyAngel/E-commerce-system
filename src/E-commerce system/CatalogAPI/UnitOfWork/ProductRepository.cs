using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.UnitOfWork;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(Context context) : base(context)
    { }
}
