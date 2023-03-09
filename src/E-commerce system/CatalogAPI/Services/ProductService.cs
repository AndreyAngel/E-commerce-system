using CatalogAPI.Services.Interfaces;
using Infrastructure.DTO;
using AutoMapper;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DataBase;
using CatalogAPI.Models.ViewModels;

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
        return _repositoryProduct.GetAll().Where(x => x.IsSale).ToList();
    }

    public Product GetById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid productId");
        }

        var res = _repositoryProduct.GetWithInclude(x => x.Id == id, x => x.Brand, y => y.Category);

        if (res == null)
        {
            throw new NotFoundException(nameof(id), "Product with this Id was not founded!");
        }

        return res;
    }

    public Product GetByName(string name)
    {
        var res = _repositoryProduct.GetWithInclude(x => x.Name == name, x => x.Brand, y => y.Category);

        if (res == null)
        {
            throw new NotFoundException(nameof(name), "Product with this name was not founded!");
        }

        return res;
    }

    public List<Product> GetByFilter(ProductFilterViewModel model)
    {
        var products = _repositoryProduct.GetAll();

        if (model.BrandId != null)
        {
            products = products.Where(x => x.BrandId == model.BrandId);
        }

        if (model.CategoryId != null)
        {
            products = products.Where(x => x.CategoryId == model.CategoryId);
        }

        return products.ToList();
    }

    public async Task<Product> Create(Product product)
    {
        if (product.Id != 0)
            product.Id = 0;

        var res = _repositoryProduct.GetByName(product.Name);

        if (res != null && res.IsSale)
        {
            throw new ObjectNotUniqueException(nameof(product.Name), "Product with this name already exists!");
        }

        else if (_repositoryBrand.GetById(product.BrandId) == null)
        {
            throw new NotFoundException(nameof(product.BrandId), "Brand with this Id was not founded!");
        }

        else if (_repositoryCategory.GetById(product.CategoryId) == null)
        {
            throw new NotFoundException(nameof(product.CategoryId), "Category with this Id was not founded!");
        }

        else if (res != null && !res.IsSale)
        {
            res.IsSale = true;
            await _repositoryProduct.UpdateAsync(res);
            return _repositoryProduct.GetWithInclude(x => x.Id == res.Id, x => x.Brand, y => y.Category);
        }

        product.IsSale = true;
        await _repositoryProduct.AddAsync(product);

        return _repositoryProduct.GetWithInclude(x => x.Id == product.Id, x => x.Brand, y => y.Category);
    }

    public async Task<Product> Update(Product product)
    {
        if (product.Id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(product.Id), "Invalid productId");
        }

        var res = _repositoryProduct.GetById(product.Id);

        if ((res.Name != product.Name) && _repositoryProduct.GetByName(product.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(product.Name), "Product with this name already exists!");
        }

        else if (res == null || !res.IsSale)
        {
            throw new NotFoundException(nameof(product.Id), "Product with this Id was not founded!");
        }
            
        else if (_repositoryBrand.GetById(product.BrandId) == null)
        {
            throw new NotFoundException(nameof(product.BrandId), "Brand with this Id was not founded!");
        }

        else if (_repositoryCategory.GetById(product.CategoryId) == null)
        {
            throw new NotFoundException(nameof(product.CategoryId), "Category with this Id was not founded!");
        }

        await _repositoryProduct.UpdateAsync(product);

        return _repositoryProduct.GetWithInclude(x => x.Id == product.Id, x => x.Brand, y => y.Category);
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
