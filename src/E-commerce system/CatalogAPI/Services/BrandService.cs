using OrderAPI.Services.Interfaces;
using OrderAPI.Exceptions;
using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.Services;

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
            throw new NotFoundException(nameof(id), "Brand with this Id was not founded!");
        }

        return res;
    }

    public Brand GetByName(string name)
    {
        var res = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Name == name);

        if (res == null)
        {
            throw new NotFoundException(nameof(name), "Brand with this name was not founded!");
        }

        return res;
    }

    public async Task<Brand> Create(Brand brand)
    {
        if (_db.Brands.GetAll().SingleOrDefault(x => x.Name == brand.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(brand.Name), "Brand with this name alredy exists!");
        }

        await _db.Brands.AddAsync(brand);

        return brand;
    }

    public async Task<Brand> Update(Brand brand)
    {
        var res = _db.Brands.GetById(brand.Id);

        if (res == null)
        {
            throw new NotFoundException(nameof(brand.Id), "Brand with this Id was not founded!");
        }

        else if ((res.Name != brand.Name) && _db.Brands.GetAll().SingleOrDefault(x => x.Name == brand.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(brand.Name), "Brand with this name already exists!");
        }

        await _db.Brands.UpdateAsync(brand);

        return brand;
    }
}
