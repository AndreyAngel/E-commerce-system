using CatalogAPI.Models.DataBase;
using CatalogAPI.Models.ViewModels;
using Infrastructure.DTO;

namespace CatalogAPI.Services.Interfaces;

public interface IProductService
{
    public List<Product> Get();

    public Product GetById(int id);

    public Product GetByName(string name);

    public List<Product> GetByFilter(ProductFilterViewModel model);

    public Task<Product> Create(Product product);

    public Task<Product> Update(Product product);

    public ProductListDTO<ProductDTO> CheckProducts(ProductListDTO<int> products);
}