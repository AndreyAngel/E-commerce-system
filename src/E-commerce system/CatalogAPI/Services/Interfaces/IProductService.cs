using CatalogAPI.Models.DataBase;
using CatalogAPI.Models.DTO;
using Infrastructure.DTO;

namespace CatalogAPI.Services.Interfaces;

public interface IProductService
{
    public List<Product> Get();

    public Product GetById(Guid id);

    public Product GetByName(string name);

    public List<Product> GetByFilter(ProductFilterDTO model);

    public Task<Product> Create(Product product);

    public Task<Product> Update(Product product);

    public ProductListDTORabbitMQ<ProductDTORabbitMQ> CheckProducts(ProductListDTORabbitMQ<Guid> products);
}