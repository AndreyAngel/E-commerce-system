using OrderAPI.Services.Interfaces;
using OrderAPI.DTO;
using AutoMapper;
using OrderAPI.Exceptions;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.Services;

public class ProductService: IProductService
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;
    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _db = unitOfWork;
        _mapper = mapper;
    }

    public List<Product> Get()
    {
        return _db.Products.GetAll().Where(x => x.IsSale).ToList();
    }

    public Product GetById(Guid id)
    {
        var res = _db.Products.Include(x => x.Category, x => x.Brand).SingleOrDefault(x => x.Id == id);

        if (res == null)
        {
            throw new NotFoundException(nameof(id), "Product with this Id was not founded!");
        }

        return res;
    }

    public Product GetByName(string name)
    {
        var res = _db.Products.Include(x => x.Name == name, x => x.Brand, y => y.Category)
                                      .SingleOrDefault(x => x.Name == name);

        if (res == null)
        {
            throw new NotFoundException(nameof(name), "Product with this name was not founded!");
        }

        return res;
    }

    public List<Product> GetByFilter(ProductFilterViewModel model)
    {
        var products = _db.Products.GetAll();

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
        var res = _db.Products.GetAll().SingleOrDefault(x => x.Name == product.Name);

        if (res != null && res.IsSale)
        {
            throw new ObjectNotUniqueException(nameof(product.Name), "Product with this name already exists!");
        }

        else if (_db.Brands.GetById(product.BrandId) == null)
        {
            throw new NotFoundException(nameof(product.BrandId), "Brand with this Id was not founded!");
        }

        else if (_db.Categories.GetById(product.CategoryId) == null)
        {
            throw new NotFoundException(nameof(product.CategoryId), "Category with this Id was not founded!");
        }

        else if (res != null && !res.IsSale)
        {
            res.IsSale = true;
            await _db.Products.UpdateAsync(res);

            return _db.Products.GetById(product.Id);
        }

        product.IsSale = true;
        await _db.Products.AddAsync(product);

        return _db.Products.Include(x => x.Category, x => x.Brand).SingleOrDefault(x => x.Id == product.Id);
    }

    public async Task<Product> Update(Product product)
    {
        var res = _db.Products.GetById(product.Id);

        if ((res.Name != product.Name) && _db.Products.GetAll().SingleOrDefault(x => x.Name == product.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(product.Name), "Product with this name already exists!");
        }

        else if (res == null || !res.IsSale)
        {
            throw new NotFoundException(nameof(product.Id), "Product with this Id was not founded!");
        }
            
        else if (_db.Brands.GetById(product.BrandId) == null)
        {
            throw new NotFoundException(nameof(product.BrandId), "Brand with this Id was not founded!");
        }

        else if (_db.Categories.GetById(product.CategoryId) == null)
        {
            throw new NotFoundException(nameof(product.CategoryId), "Category with this Id was not founded!");
        }

        await _db.Products.UpdateAsync(product);

        return _db.Products.Include(x => x.Category, x => x.Brand).SingleOrDefault(x => x.Id == product.Id);
    }

    // Returns actuality products by ID
    public ProductListDTO<ProductDTO> CheckProducts(ProductListDTO<Guid> productList)
    {
        ProductListDTO<ProductDTO> products = new();
        foreach (var productId in productList.Products)
        {
            var product = _db.Products.GetById(productId);

            if (product != null && product.IsSale)
                products.Products.Add(_mapper.Map<ProductDTO>(product));
            else
                products.Products.Add(null);
        }

        return products;
    }
}
