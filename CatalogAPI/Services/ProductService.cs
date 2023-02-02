using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DTO;

namespace CatalogAPI.Services;

public class ProductService: IProductService
{
    private readonly Context _db;
    public ProductService(Context context)
    {
        _db = context;
    }

    public async Task<List<Product>> Get()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await _db.Products.Include(x => x.Category).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Product> GetByName(string name)
    {
        return await _db.Products.Include(x => x.Category).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Product>> GetByBrandId(int brandId)
    {
        return await _db.Products.Where(x => x.Brand.Id == brandId).ToListAsync();
    }

    public async Task<List<Product>> GetByBrandName(string brandName)
    {
        return await _db.Products.Where(x => x.Brand.Name == brandName).ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryId(int categoryId)
    {
        return await _db.Products.Where(x => x.Category.Id == categoryId).ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryName(string categoryName)
    {
        return await _db.Products.Where(x => x.Category.Name == categoryName).ToListAsync();
    }

    public async Task<Product> Create(Product product)
    {
        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task<ProductList> CheckProducts(ProductList productList)
    {
        return new ProductList();
        //TODO
    }
}
