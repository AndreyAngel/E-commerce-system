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
        return await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Category> GetByName(string name)
    {
        return await _db.Categories.SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Category> Create(Category category)
    {
        await _db.Categories.AddAsync(category);
        await _db.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
        return category;
    }
}
