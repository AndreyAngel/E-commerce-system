using CatalogAPI.Models;

namespace CatalogAPI.Services.Interfaces;

public interface ICategoryService
{
    public Task<List<Category>> Get();
    public Task<Category> Get(int id);
    public Task<Category> Get(string name);
    public Task<Category> Create(Category category);
    public Task<Category> Update(Category category);
}
