using CatalogAPI.Models;

namespace CatalogAPI.Services.Interfaces;

public interface ICategoryService
{
    public Task<List<Category>> Get();
    public Task<Category> GetById(int id);
    public Task<Category> GetByName(string name);
    public Task<Category> Create(Category category);
    public Task<Category> Update(Category category);
}
