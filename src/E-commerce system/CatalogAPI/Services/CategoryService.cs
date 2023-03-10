using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DataBase;
using Infrastructure.Models;

namespace CatalogAPI.Services;

public class CategoryService: ICategoryService
{
    private readonly IRepositoryService<Category> _repositoryService;
    public CategoryService(IRepositoryService<Category> repositoryService)
    {
        _repositoryService = repositoryService;
    }

    public List<Category> Get()
    {
        return _repositoryService.GetAll().ToList();
    }

    public Category GetById(int id)
    {

        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid categoryId");
        }

        var res = _repositoryService.GetById(id);

        if (res == null)
        {
            throw new NotFoundException(nameof(id), "Category with this Id was not founded!");
        }

        return res;
    }

    public Category GetByName(string name)
    {
        var res = _repositoryService.GetByName(name);

        if (res == null)
        {
            throw new NotFoundException(nameof(name), "Category with this name was not founded!");
        }

        return res;
    }

    public async Task<Category> Create(Category category)
    {
        if (category.Id != 0)
            category.Id = 0;

        if (_repositoryService.GetByName(category.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(category.Name), "Category with this name alredy exists!");
        }

        await _repositoryService.AddAsync(category);

        return category;
    }

    public async Task<Category> Update(Category category)
    {
        if (category.Id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(category.Id), "Invalid categoryId");
        }

        var res = _repositoryService.GetById(category.Id);

        if (res == null)
        {
            throw new NotFoundException(nameof(category.Id), "Category with this Id was not founded!");
        }
            
        else if ((res.Name != category.Name) && _repositoryService.GetByName(category.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(category.Name), "Category with this name already exists!");
        }

        await _repositoryService.UpdateAsync(category);

        return category;
    }
}
