using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    public BrandRepository(Context context) : base(context)
    { }
}
