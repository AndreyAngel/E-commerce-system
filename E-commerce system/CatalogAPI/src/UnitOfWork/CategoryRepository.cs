using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.UnitOfWork;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(Context context) : base(context)
    { }
}
