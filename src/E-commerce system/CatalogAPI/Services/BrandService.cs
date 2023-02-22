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
        if (id <= 0)
            throw new Exception("id <= 0"); //todo: new exception

        var res = await _db.Brands.SingleOrDefaultAsync(x => x.Id == id);

        if (res == null)
            throw new Exception("Brand not found!"); //todo: new exception

        return res;
    }

    public async Task<Brand> GetByName(string name)
    {
        var res = await _db.Brands.SingleOrDefaultAsync(x => x.Name == name);

        if (res == null)
            throw new Exception("Brand not found!"); //todo: new exception

        return res;
    }

    public async Task<Brand> Create(Brand brand)
    {
        if (brand.Id != 0)
            throw new Exception("Нельзя передавать id!"); //todo: new exception

        if (GetByName(brand.Name) != null)
            throw new Exception("Brand with this name alredy exists!");//todo: new exception

        await _db.Brands.AddAsync(brand);
        await _db.SaveChangesAsync();
        return brand;
    }

    public async Task<Brand> Update(Brand brand)
    {
        if (brand.Id <= 0)
            throw new Exception("id <= 0"); //todo: new exception

        if (GetById(brand.Id) == null)
            throw new Exception("Brand bot found!");//todo: new exception

        _db.Brands.Update(brand);
        await _db.SaveChangesAsync();
        return brand;
    }
}
