using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(Context context) : base(context)
    { }
}
