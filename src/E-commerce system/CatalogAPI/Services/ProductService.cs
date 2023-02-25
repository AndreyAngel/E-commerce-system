using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DTO;
using AutoMapper;
using Infrastructure.Exceptions;

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
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid productId");

        var res = await _db.Products.Include(x => x.Category).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Id == id);

        if (res == null)
            throw new NotFoundException(nameof(id), "Product with this Id was not founded!");

        return res;
    }

    public async Task<Product> GetByName(string name)
    {
        var res = await _db.Products.Include(x => x.Category).Include(x => x.Brand).SingleOrDefaultAsync(x => x.Name == name);

        if (res == null)
            throw new NotFoundException(nameof(name), "Product with this name was not founded!");

        return res;
    }

    public async Task<List<Product>> GetByBrandId(int brandId)
    {
        if (brandId <= 0)
            throw new ArgumentOutOfRangeException(nameof(brandId), "Invalid productId");

        var res = _db.Brands.SingleOrDefaultAsync(x => x.Id == brandId);

        if (res == null)
            throw new NotFoundException(nameof(brandId), "Brand with this Id was not founded!");

        return await _db.Products.Where(x => x.Brand.Id == brandId).ToListAsync();
    }

    public async Task<List<Product>> GetByBrandName(string brandName)
    {
        var res = _db.Brands.SingleOrDefaultAsync(x => x.Name == brandName);

        if (res == null)
            throw new NotFoundException(nameof(brandName), "Brand with this name was not founded!");

        return await _db.Products.Where(x => x.Brand.Name == brandName).ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryId(int categoryId)
    {
        if (categoryId <= 0)
            throw new ArgumentOutOfRangeException(nameof(categoryId), "Invalid productId");

        var res = _db.Categories.SingleOrDefaultAsync(x => x.Id == categoryId);

        if (res == null)
            throw new NotFoundException(nameof(categoryId), "Category with this Id was not founded!");

        return await _db.Products.Where(x => x.Category.Id == categoryId).ToListAsync();
    }

    public async Task<List<Product>> GetByCategoryName(string categoryName)
    {
        var res = _db.Categories.SingleOrDefaultAsync(x => x.Name == categoryName);

        if (res == null)
            throw new NotFoundException(nameof(categoryName), "Category with this name was not founded!");

        return await _db.Products.Where(x => x.Category.Name == categoryName).ToListAsync();
    }

    public async Task<Product> Create(Product product)
    {
        if (product.Id != 0)
            product.Id = 0;

        if (await GetByName(product.Name) != null)
            throw new ObjectNotUniqueException(nameof(product.Name), "Product with this name already exists!");

        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        if (product.Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(product.Id), "Invalid productId");

        if (await GetById(product.Id) == null)
            throw new NotFoundException(nameof(product.Id), "Product with this Id was not founded!");

        _db.Products.Update(product);
        await _db.SaveChangesAsync();
        return product;
    }

    // Returns actuality products by ID
    public async Task<ProductListDTO<ProductDTO>> CheckProducts(ProductListDTO<int> productList)
    {
        ProductListDTO<ProductDTO> products = new();
        foreach (var productId in productList.Products)
        {
            var product = await _db.Products.SingleOrDefaultAsync(x => x.Id == productId);

            if (product != null && product.IsSale)
                products.Products.Add(_mapper.Map<ProductDTO>(product));
            else
                products.Products.Add(null);
        }

        return products;
    }
}
