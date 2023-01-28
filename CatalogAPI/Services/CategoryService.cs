using CatalogAPI.Services.Interfaces;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Services;

public class CategoryService: ICategoryService
{
    private Context db;
    public CategoryService(Context context)
    {
        db = context;
    }

    public async Task<List<Category>> Get()
    {
        return await db.Categories.ToListAsync();
    }

    public async Task<Category> Get(int id)
    {
        return await db.Categories.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Category> Get(string name)
    {
        return await db.Categories.SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Category> Create(Category category)
    {
        await db.Categories.AddAsync(category);
        await db.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        db.Categories.Update(category);
        await db.SaveChangesAsync();
        return category;
    }
}
