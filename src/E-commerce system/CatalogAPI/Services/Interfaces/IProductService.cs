using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels;
using OrderAPI.DTO;

namespace OrderAPI.Services.Interfaces;

public interface IProductService
{
    public List<Product> Get();

    public Product GetById(Guid id);

    public Product GetByName(string name);

    public List<Product> GetByFilter(ProductFilterViewModel model);

    public Task<Product> Create(Product product);

    public Task<Product> Update(Product product);

    public ProductListDTO<ProductDTO> CheckProducts(ProductListDTO<Guid> products);
}