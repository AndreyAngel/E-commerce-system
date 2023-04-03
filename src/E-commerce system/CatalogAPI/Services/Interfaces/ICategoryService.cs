using OrderAPI.Models.DataBase;

namespace OrderAPI.Services.Interfaces;

public interface ICategoryService
{
    public List<Category> Get();
    public Category GetById(Guid id);
    public Category GetByName(string name);
    public Task<Category> Create(Category category);
    public Task<Category> Update(Category category);
}
