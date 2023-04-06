using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.UnitOfWork;

public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    public BrandRepository(Context context) : base(context)
    { }
}
