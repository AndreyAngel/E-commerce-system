using CatalogAPI.Models;

namespace CatalogAPI.Services.Interfaces;
interface IBrandService
{
    public Task<List<Brand>> Get();
    public Task<Brand> Get(int id);
    public Task<Brand> Get(string name);
    public Task<Brand> Create(Brand brand);
    public Task<Brand> Update(Brand brand);
}

