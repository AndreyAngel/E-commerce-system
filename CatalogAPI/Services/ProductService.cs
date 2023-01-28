using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Services;

public class ProductService: IProductService
{
    private Context db;
    public ProductService(Context context)
    {
        db = context;
    }

    public async Task<List<Product>> Get()
    {
        return await db.Products.ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await db.Products.Include(x => x.Category).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Product> GetByName(string name)
    {
        return await db.Products.Include(x => x.Category).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Product>> GetByBrandId(int brandId)
    {
        return await db.Products.Include(x => x.Category).Include(x => x.Brand).Where(x => x.Brand.Id == brandId).ToListAsync();
    }

    public async Task<List<Product>> GetByBrandName(string brandName)
    {
        return await db.Products.Include(x => x.Category).Include(x => x.Brand).Where(x => x.Brand.Name == brandName).ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryId(int categoryId)
    {
        return await db.Products.Include(x => x.Category).Include(x => x.Brand).Where(x => x.Category.Id == categoryId).ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryName(string categoryName)
    {
        return await db.Products.Include(x => x.Category).Include(x => x.Brand).Where(x => x.Category.Name == categoryName).ToListAsync();
    }

    public async Task<Product> Create(Product product)
    {
        await db.Products.AddAsync(product);
        await db.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        db.Products.Update(product);
        await db.SaveChangesAsync();
        return product;
    }
}
