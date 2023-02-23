using CatalogAPI.Services.Interfaces;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Services;

public class CategoryService: ICategoryService
{
    private readonly Context _db;
    public CategoryService(Context context)
    {
        _db = context;
    }

    public async Task<List<Category>> Get()
    {
        return await _db.Categories.ToListAsync();
    }

    public async Task<Category> GetById(int id)
    {

        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));

        var res = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);

        if (res == null)
            throw new Exception("Category not found!"); //todo: new exception

        return res;
    }

    public async Task<Category> GetByName(string name)
    {
        var res = await _db.Categories.SingleOrDefaultAsync(x => x.Name == name);

        if (res == null)
            throw new Exception("Category not found!"); //todo: new exception

        return res;
    }

    public async Task<Category> Create(Category category)
    {
        if (category.Id != 0)
            category.Id = 0;

        if (await GetByName(category.Name) != null)
            throw new Exception("Category with this name alredy exists!");//todo: new exception

        await _db.Categories.AddAsync(category);
        await _db.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        if (category.Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(category.Id));

        if (await GetById(category.Id) == null)
            throw new Exception("Category bot found!");//todo: new exception

        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
        return category;
    }
}
