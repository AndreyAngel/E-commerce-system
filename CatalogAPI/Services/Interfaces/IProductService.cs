using CatalogAPI.Models;
using Infrastructure.DTO;

namespace CatalogAPI.Services.Interfaces;

public interface IProductService
{
    public Task<List<Product>> Get();
    public Task<Product> GetById(int id);
    public Task<Product> GetByName(string name);
    public Task<List<Product>> GetByBrandId(int brandId);
    public Task<List<Product>> GetByBrandName(string brandName);
    public Task<List<Product>> GetByCategoryId(int categoryId);
    public Task<List<Product>> GetByCategoryName(string categoryName);
    public Task<Product> Create(Product product);
    public Task<Product> Update(Product product);
    public Task<ProductList> CheckProducts(ProductList products);
}
