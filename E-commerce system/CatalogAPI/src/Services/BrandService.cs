using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.Services;

public class BrandService: IBrandService
{
    private readonly IUnitOfWork _db;
    public BrandService(IUnitOfWork unitOfWork)
    {
        _db = unitOfWork;
    }

    public List<Brand> Get()
    {
        return _db.Brands.GetAll().ToList();
    }

    public Brand GetById(Guid id)
    {
        var res = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Id == id);

        if (res == null)
        {
            throw new NotFoundException("Brand with this Id was not founded!", nameof(id));
        }

        return res;
    }

    public Brand GetByName(string name)
    {
        var res = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Name == name);

        if (res == null)
        {
            throw new NotFoundException("Brand with this name was not founded!", nameof(name));
        }

        return res;
    }

    public async Task<Brand> Create(Brand brand)
    {
        if (_db.Brands.GetAll().SingleOrDefault(x => x.Name == brand.Name) != null)
        {
            throw new ObjectNotUniqueException("Brand with this name alredy exists!", nameof(brand.Name));
        }

        await _db.Brands.AddAsync(brand);

        return brand;
    }

    public async Task<Brand> Update(Brand brand)
    {
        var res = _db.Brands.GetById(brand.Id);

        if (res == null)
        {
            throw new NotFoundException("Brand with this Id was not founded!", nameof(brand.Id));
        }

        else if ((res.Name != brand.Name) && _db.Brands.GetAll().SingleOrDefault(x => x.Name == brand.Name) != null)
        {
            throw new ObjectNotUniqueException("Brand with this name already exists!", nameof(brand.Name));
        }

        await _db.Brands.UpdateAsync(brand);

        return brand;
    }
}
