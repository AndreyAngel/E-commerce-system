using CatalogAPI.Services.Interfaces;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Exceptions;

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
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid categoryId");

        var res = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);

        if (res == null)
            throw new NotFoundException(nameof(id), "Category with this Id was not founded!");

        return res;
    }

    public async Task<Category> GetByName(string name)
    {
        var res = await _db.Categories.SingleOrDefaultAsync(x => x.Name == name);

        if (res == null)
            throw new NotFoundException(nameof(name), "Category with this name was not founded!");

        return res;
    }

    public async Task<Category> Create(Category category)
    {
        if (category.Id != 0)
            category.Id = 0;

        if (await GetByName(category.Name) != null)
            throw new ObjectNotUniqueException(nameof(category.Name), "Category with this name alredy exists!");

        await _db.Categories.AddAsync(category);
        await _db.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        if (category.Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(category.Id), "Invalid categoryId");

        if (await GetById(category.Id) == null)
            throw new NotFoundException(nameof(category.Id), "Category with this Id was not founded!");

        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
        return category;
    }
}
