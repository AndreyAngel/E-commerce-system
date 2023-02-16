using CatalogAPI.Services.Interfaces;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Services;

public class BrandService: IBrandService
{
    private readonly Context _db;
    public BrandService(Context context)
    {
        _db = context;
    }

    public async Task<List<Brand>> Get()
    {
        return await _db.Brands.ToListAsync();
    }

    public async Task<Brand> GetById(int id)
    {
        return await _db.Brands.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Brand> GetByName(string name)
    {
        return await _db.Brands.SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Brand> Create(Brand brand)
    {
        await _db.Brands.AddAsync(brand);
        await _db.SaveChangesAsync();
        return brand;
    }

    public async Task<Brand> Update(Brand brand)
    {
        _db.Brands.Update(brand);
        await _db.SaveChangesAsync();
        return brand;
    }
}
