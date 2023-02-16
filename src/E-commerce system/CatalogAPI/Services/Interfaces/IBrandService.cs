using CatalogAPI.Models;

namespace CatalogAPI.Services.Interfaces;
public interface IBrandService
{
    public Task<List<Brand>> Get();
    public Task<Brand> GetById(int id);
    public Task<Brand> GetByName(string name);
    public Task<Brand> Create(Brand brand);
    public Task<Brand> Update(Brand brand);
}

