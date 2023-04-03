using OrderAPI.Models.DataBase;

namespace OrderAPI.Services.Interfaces;
public interface IBrandService
{
    public List<Brand> Get();
    public Brand GetById(Guid id);
    public Brand GetByName(string name);
    public Task<Brand> Create(Brand brand);
    public Task<Brand> Update(Brand brand);
}

