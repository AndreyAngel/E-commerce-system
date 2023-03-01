using CatalogAPI.Models;

namespace CatalogAPI.Services.Interfaces;
public interface IBrandService
{
    public List<Brand> Get();
    public Brand GetById(int id);
    public Brand GetByName(string name);
    public Task<Brand> Create(Brand brand);
    public Task<Brand> Update(Brand brand);
}

