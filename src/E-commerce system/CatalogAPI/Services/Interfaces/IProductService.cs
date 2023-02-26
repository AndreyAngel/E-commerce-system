using CatalogAPI.Models;
using Infrastructure.DTO;

namespace CatalogAPI.Services.Interfaces;

public interface IProductService
{
    public List<Product> Get();
    public Product GetById(int id);
    public Product GetByName(string name);
    public List<Product> GetByBrandId(int brandId);
    public List<Product> GetByBrandName(string brandName);
    public List<Product> GetByCategoryId(int categoryId);
    public List<Product> GetByCategoryName(string categoryName);
    public Task<Product> Create(Product product);
    public Task<Product> Update(Product product);
    public ProductListDTO<Infrastructure.DTO.ProductDTO> CheckProducts(ProductListDTO<int> products);
}