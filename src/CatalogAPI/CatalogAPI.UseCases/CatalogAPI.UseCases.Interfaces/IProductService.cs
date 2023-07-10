using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Entities;
using Infrastructure.DTO;
using Infrastructure.Exceptions;

namespace CatalogAPI.UseCases.Interfaces;

/// <summary>
/// Interface for class providing the APIs for managing product in a persistence store.
/// </summary>
public interface IProductService : IDisposable
{
    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns> Products list, <see cref="List{Product}"/> </returns>
    public List<Product> GetAll();

    /// <summary>
    /// Get product by Id
    /// </summary>
    /// <param name="id"> Product Id </param>
    /// <returns> <see cref="Product"/> </returns>
    /// <exception cref="NotFoundException"> Product with this Id wasn't founded </exception>
    public Product GetById(Guid id);

    /// <summary>
    /// Get product by name
    /// </summary>
    /// <param name="name"> Product name </param>
    /// <returns> <see cref="Product"/> </returns>
    /// <exception cref="NotFoundException"> Product with this name wasn't founded </exception>
    public Product GetByName(string name);

    /// <summary>
    /// Get product by filters
    /// </summary>
    /// <param name="model"> Model with filters </param>
    /// <returns> Products list, <see cref="List{Product}"/> </returns>
    public List<Product> GetByFilter(ProductFilterDTO model);

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="product"> New product </param>
    /// <returns> Task containing a created product, <see cref="Product"/> </returns>
    /// <exception cref="ObjectNotUniqueException"> Product with this name alredy exists </exception>
    /// <exception cref="NotFoundException"> Product with this name or Id wasn't founded </exception>
    public Task<Product> Create(Product product);

    /// <summary>
    /// Change product data
    /// </summary>
    /// <param name="product"> Product data for changing </param>
    /// <returns> Task containing a chenged product, <see cref="Product"/> </returns>
    /// <exception cref="NotFoundException"> Product with this name wasn't founded </exception>
    /// <exception cref="ObjectNotUniqueException"> Product with this name alredy exists </exception>
    public Task<Product> Update(Product product);

    /// <summary>
    /// Check products data for relevance
    /// </summary>
    /// <param name="products"> List of product IDs coming from an external service </param>
    /// <returns> List of actuality products, <see cref="ProductListDTORabbitMQ{ProductDTORabbitMQ}"/> </returns>
    public ProductListDTORabbitMQ<ProductDTORabbitMQ> GetActualityProducts(ProductListDTORabbitMQ<Guid> products);
}