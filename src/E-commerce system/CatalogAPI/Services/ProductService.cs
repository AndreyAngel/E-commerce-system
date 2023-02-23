using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DTO;
using AutoMapper;

namespace CatalogAPI.Services;

public class ProductService: IProductService
{
    private readonly Context _db;
    private readonly IMapper _mapper;
    public ProductService(Context context, IMapper mapper)
    {
        _db = context;
        _mapper = mapper;
    }

    public async Task<List<Product>> Get()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        if (id <= 0)
            throw new Exception("id <= 0"); // todo: new exception

        var res = await _db.Products.Include(x => x.Category).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Id == id);

        if (res == null)
            throw new Exception("Product not found!"); // todo: new exception

        return res;
    }

    public async Task<Product> GetByName(string name)
    {
        var res = await _db.Products.Include(x => x.Category).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Name == name);

        if (res == null)
            throw new Exception("Product not found!"); // todo: new exception

        return res;
    }

    public async Task<List<Product>> GetByBrandId(int brandId)
    {
        var res = _db.Brands.SingleOrDefaultAsync(x => x.Id == brandId);

        if (res == null)
            throw new Exception("Brand not found!"); // todo: new exception

        return await _db.Products.Where(x => x.Brand.Id == brandId).ToListAsync();
    }

    public async Task<List<Product>> GetByBrandName(string brandName)
    {
        var res = _db.Brands.SingleOrDefaultAsync(x => x.Name == brandName);

        if (res == null)
            throw new Exception("Brand not found!"); // todo: new exception

        return await _db.Products.Where(x => x.Brand.Name == brandName).ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryId(int categoryId)
    {
        var res = _db.Categories.SingleOrDefaultAsync(x => x.Id == categoryId);

        if (res == null)
            throw new Exception("Category not found!"); // todo: new exception

        return await _db.Products.Where(x => x.Category.Id == categoryId).ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryName(string categoryName)
    {
        var res = _db.Categories.SingleOrDefaultAsync(x => x.Name == categoryName);

        if (res == null)
            throw new Exception("Category not found!"); // todo: new exception

        return await _db.Products.Where(x => x.Category.Name == categoryName).ToListAsync();
    }

    public async Task<Product> Create(Product product)
    {
        if (product.Id != 0)
            throw new Exception("Нельзя передавать id!"); //todo: new exception

        if (GetByName(product.Name) != null)
            throw new Exception("Product with this name already exists!"); //todo: new exception

        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        if (product.Id <= 0)
            throw new Exception("id <= 0"); //todo: new exception

        if (GetById(product.Id) == null)
            throw new Exception("Product bot found!");//todo: new exception

        _db.Products.Update(product);
        await _db.SaveChangesAsync();
        return product;
    }

    // Returns actuality products by ID
    public async Task<ProductList<Infrastructure.Models.Product>> CheckProducts(ProductList<int> productList)
    {
        ProductList<Infrastructure.Models.Product> products = new();
        foreach (var productId in productList.Products)
        {
            var product = await _db.Products.SingleOrDefaultAsync(x => x.Id == productId);

            if (product != null && product.IsSale)
                products.Products.Add(_mapper.Map<Infrastructure.Models.Product>(product));
            else
                products.Products.Add(null);
        }

        return products;
    }
}
