using Infrastructure.DTO;
using AutoMapper;
using Infrastructure.Exceptions;
using CatalogAPI.UseCases.Interfaces;
using CatalogAPI.Domain.Repositories.Interfaces;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Contracts.DTO;

namespace CatalogAPI.UseCases.Implementation;

/// <summary>
/// Сlass providing the APIs for managing product in a persistence store.
/// </summary>
public class ProductService: IProductService
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _db;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Creates an instance of the <see cref="ProductService"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    ~ProductService() => Dispose(false);

    /// <inheritdoc/>
    public List<Product> GetAll()
    {
        ThrowIfDisposed();

        return _db.Products.GetAll().Where(x => x.IsSale).ToList();
    }

    /// <inheritdoc/>
    public Product GetById(Guid id)
    {
        ThrowIfDisposed();

        var product = _db.Products.Include(x => x.Category, x => x.Brand).SingleOrDefault(x => x.Id == id);

        if (product == null)
        {
            throw new NotFoundException("Product with this Id was not founded!", nameof(id));
        }

        return product;
    }

    /// <inheritdoc/>
    public Product GetByName(string name)
    {
        ThrowIfDisposed();

        var product = _db.Products.Include(x => x.Name == name, x => x.Category, y => y.Brand)
                                      .SingleOrDefault(x => x.Name == name);

        if (product == null)
        {
            throw new NotFoundException("Product with this name was not founded!", nameof(name));
        }

        return product;
    }

    /// <inheritdoc/>
    public List<Product> GetByFilter(ProductFilterDTO model)
    {
        ThrowIfDisposed();

        var products = _db.Products.GetAll();


        if (model.BrandId != null)
        {
            products = products.Where(x => x.BrandId == model.BrandId).ToList();
        }

        if (model.CategoryId != null)
        {
            products = products.Where(x => x.CategoryId == model.CategoryId).ToList();
        }

        return products.ToList();
    }

    /// <inheritdoc/>
    public async Task<Product> Create(Product product)
    {
        ThrowIfDisposed();
        var res = _db.Products.GetAll().SingleOrDefault(x => x.Name == product.Name);

        if (res != null && res.IsSale)
        {
            throw new ObjectNotUniqueException("Product with this name already exists!", nameof(product.Name));
        }

        else if (_db.Brands.GetById(product.BrandId) == null)
        {
            throw new NotFoundException("category with this Id was not founded!", nameof(product.BrandId));
        }

        else if (_db.Categories.GetById(product.CategoryId) == null)
        {
            throw new NotFoundException("category with this Id was not founded!", nameof(product.CategoryId));
        }

        else if (res != null && !res.IsSale)
        {
            res.IsSale = true;
            await _db.Products.UpdateAsync(res);

            return _db.Products.GetById(product.Id);
        }

        product.IsSale = true;
        await _db.Products.AddAsync(product);

        product.Category = _db.Categories.GetById(product.CategoryId);
        product.Brand = _db.Brands.GetById(product.BrandId);

        return product;
    }

    /// <inheritdoc/>
    public async Task<Product> Update(Product product)
    {
        ThrowIfDisposed();
        var res = _db.Products.GetById(product.Id);

        if ((res.Name != product.Name) && _db.Products.GetAll().SingleOrDefault(x => x.Name == product.Name) != null)
        {
            throw new ObjectNotUniqueException("Product with this name already exists!", nameof(product.Name));
        }

        else if (res == null || !res.IsSale)
        {
            throw new NotFoundException("Product with this Id was not founded!", nameof(product.Id));
        }
            
        else if (_db.Brands.GetById(product.BrandId) == null)
        {
            throw new NotFoundException("category with this Id was not founded!", nameof(product.BrandId));
        }

        else if (_db.Categories.GetById(product.CategoryId) == null)
        {
            throw new NotFoundException("category with this Id was not founded!", nameof(product.CategoryId));
        }

        await _db.Products.UpdateAsync(product);

        return _db.Products.Include(x => x.Brand, x => x.Category).SingleOrDefault(x => x.Id == product.Id);
    }

    // Returns actuality products by ID
    /// <inheritdoc/>
    public ProductListDTORabbitMQ<ProductDTORabbitMQ> GetActualityProducts(ProductListDTORabbitMQ<Guid> productList)
    {
        ThrowIfDisposed();
        ProductListDTORabbitMQ<ProductDTORabbitMQ> products = new();

        foreach (var productId in productList.Products)
        {
            var product = _db.Products.GetById(productId);

            if (product != null && product.IsSale)
                products.Products.Add(_mapper.Map<ProductDTORabbitMQ>(product));
            else
                products.Products.Add(null);
        }

        return products;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}
