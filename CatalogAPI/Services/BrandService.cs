using CatalogAPI.Services.Interfaces;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Services;

public class BrandService: IBrandService
{
    private Context db;
    public BrandService(Context context)
    {
        db = context;
    }

    public async Task<List<Brand>> Get()
    {
        return await db.Brands.ToListAsync();
    }

    public async Task<Brand> Get(int id)
    {
        return await db.Brands.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Brand> Get(string name)
    {
        return await db.Brands.SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Brand> Create(Brand brand)
    {
        await db.Brands.AddAsync(brand);
        await db.SaveChangesAsync();
        return brand;
    }

    public async Task<Brand> Update(Brand brand)
    {
        db.Brands.Update(brand);
        await db.SaveChangesAsync();
        return brand;
    }
}
