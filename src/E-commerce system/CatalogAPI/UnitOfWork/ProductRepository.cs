using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.UnitOfWork;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(Context context) : base(context)
    { }
}
