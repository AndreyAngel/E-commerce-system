using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DataBase;

namespace CatalogAPI.Services;

public class BrandService: IBrandService
{
    private readonly IRepositoryService<Brand> _repositoryService;
    public BrandService(IRepositoryService<Brand> repositoryService)
    {
        _repositoryService = repositoryService;
    }

    public List<Brand> Get()
    {
        return _repositoryService.GetAll().ToList();
    }

    public Brand GetById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid BrandId");
        }

        var res = _repositoryService.GetById(id);

        if (res == null)
        {
            throw new NotFoundException(nameof(id), "Brand with this Id was not founded!");
        }

        return res;
    }

    public Brand GetByName(string name)
    {
        var res = _repositoryService.GetByName(name);

        if (res == null)
        {
            throw new NotFoundException(nameof(name), "Brand with this name was not founded!");
        }

        return res;
    }

    public async Task<Brand> Create(Brand brand)
    {
        if (brand.Id != 0)
            brand.Id = 0;

        if (_repositoryService.GetByName(brand.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(brand.Name), "Brand with this name alredy exists!");
        }

        await _repositoryService.AddAsync(brand);

        return brand;
    }

    public async Task<Brand> Update(Brand brand)
    {
        if (brand.Id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(brand.Id), "Invalid brandId");
        }

        var res = _repositoryService.GetById(brand.Id);

        if (res == null)
        {
            throw new NotFoundException(nameof(brand.Id), "Brand with this Id was not founded!");
        }

        else if ((res.Name != brand.Name) && _repositoryService.GetByName(brand.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(brand.Name), "Brand with this name already exists!");
        }

        await _repositoryService.UpdateAsync(brand);

        return brand;
    }
}
