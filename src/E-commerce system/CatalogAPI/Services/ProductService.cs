using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Infrastructure.DTO;
using AutoMapper;
using Infrastructure.Exceptions;

namespace CatalogAPI.Services;

public class ProductService: IProductService
{
    private readonly IRepositoryService<Product> _repositoryProduct;
    private readonly IRepositoryService<Category> _repositoryCategory;
    private readonly IRepositoryService<Brand> _repositoryBrand;
    private readonly IMapper _mapper;
    public ProductService(IRepositoryService<Product> repositoryProduct,
                          IRepositoryService<Category> repositoryCategory,
                          IRepositoryService<Brand> repositoryBrand,
                          IMapper mapper)
    {
        _repositoryProduct = repositoryProduct;
        _repositoryCategory = repositoryCategory;
        _repositoryBrand = repositoryBrand;
        _mapper = mapper;
    }

    public List<Product> Get()
    {
        return _repositoryProduct.GetAll().ToList();
    }

    public Product GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid productId");

        var res = _repositoryProduct.GetWithInclude(x => x.Brand, y => y.Category).FirstOrDefault(x => x.Id == id);

        if (res == null)
            throw new NotFoundException(nameof(id), "Product with this Id was not founded!");

        return res;
    }

    public Product GetByName(string name)
    {
        var res = _repositoryProduct.GetWithInclude(x => x.Brand, y => y.Category).FirstOrDefault(x => x.Name == name);

        if (res == null)
            throw new NotFoundException(nameof(name), "Product with this name was not founded!");

        return res;
    }

    public List<Product> GetByBrandId(int brandId)
    {
        if (brandId <= 0)
            throw new ArgumentOutOfRangeException(nameof(brandId), "Invalid productId");

        var brand = _repositoryBrand.GetById(brandId);

        if (brand == null)
            throw new NotFoundException(nameof(brandId), "Brand with this Id was not founded!");

        return _repositoryProduct.GetWithInclude(x => x.BrandId == brandId, x => x.Brand, x => x.Category).ToList();
    }

    public List<Product> GetByBrandName(string brandName)
    {
        var brand = _repositoryBrand.GetByName(brandName);

        if (brand == null)
            throw new NotFoundException(nameof(brandName), "Brand with this name was not founded!");

        return _repositoryProduct.GetWithInclude(x => x.BrandId == brand.Id, x => x.Brand, x => x.Category).ToList();
    }

    public List<Product> GetByCategoryId(int categoryId)
    {
        if (categoryId <= 0)
            throw new ArgumentOutOfRangeException(nameof(categoryId), "Invalid productId");

        var category = _repositoryCategory.GetById(categoryId);

        if (category == null)
            throw new NotFoundException(nameof(categoryId), "Category with this Id was not founded!");

        return _repositoryProduct.GetWithInclude(x => x.CategoryId == categoryId, x => x.Brand, x => x.Category).ToList();
    }

    public List<Product> GetByCategoryName(string categoryName)
    {
        var category = _repositoryCategory.GetByName(categoryName);

        if (category == null)
            throw new NotFoundException(nameof(categoryName), "Category with this name was not founded!");

        return _repositoryProduct.GetWithInclude(x => x.CategoryId == category.Id, x => x.Brand, x => x.Category).ToList();
    }

    public async Task<Product> Create(Product product)
    {
        if (product.Id != 0)
            product.Id = 0;

        if (_repositoryProduct.GetById(product.Id) != null)
            throw new ObjectNotUniqueException(nameof(product.Name), "Product with this name already exists!");

        await _repositoryProduct.AddAsync(product);

        return product;
    }

    public async Task<Product> Update(Product product)
    {
        if (product.Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(product.Id), "Invalid productId");

        if (_repositoryProduct.GetById == null)
            throw new NotFoundException(nameof(product.Id), "Product with this Id was not founded!");

        await _repositoryProduct.UpdateAsync(product);

        return product;
    }

    // Returns actuality products by ID
    public ProductListDTO<ProductDTO> CheckProducts(ProductListDTO<int> productList)
    {
        ProductListDTO<ProductDTO> products = new();
        foreach (var productId in productList.Products)
        {
            var product = _repositoryProduct.GetById(productId);

            if (product != null && product.IsSale)
                products.Products.Add(_mapper.Map<ProductDTO>(product));
            else
                products.Products.Add(null);
        }

        return products;
    }
}
