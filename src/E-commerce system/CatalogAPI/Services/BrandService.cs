using CatalogAPI.Services.Interfaces;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using CatalogAPI.Exceptions;

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
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid BrandId");

        var res = await _db.Brands.SingleOrDefaultAsync(x => x.Id == id);

        if (res == null)
            throw new NotFoundException(nameof(id), "Brand with this Id was not founded!");

        return res;
    }

    public async Task<Brand> GetByName(string name)
    {
        var res = await _db.Brands.SingleOrDefaultAsync(x => x.Name == name);

        if (res == null)
            throw new NotFoundException(nameof(name), "Brand with this name was not founded!");

        return res;
    }

    public async Task<Brand> Create(Brand brand)
    {
        if (brand.Id != 0)
            brand.Id = 0;

        if (await GetByName(brand.Name) != null)
            throw new ObjectNotUniqueException(nameof(brand.Name), "Brand with this name alredy exists!");

        await _db.Brands.AddAsync(brand);
        await _db.SaveChangesAsync();
        return brand;
    }

    public async Task<Brand> Update(Brand brand)
    {
        if (brand.Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(brand.Id), "Invalid brandId");

        if (await GetById(brand.Id) == null)
            throw new NotFoundException(nameof(brand.Id), "Brand with this Id was not founded!");

        _db.Brands.Update(brand);
        await _db.SaveChangesAsync();
        return brand;
    }
}
