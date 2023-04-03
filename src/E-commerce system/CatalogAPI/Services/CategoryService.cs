using OrderAPI.Services.Interfaces;
using OrderAPI.Exceptions;
using OrderAPI.Models.DataBase;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.Services;

public class CategoryService: ICategoryService
{
    private readonly IUnitOfWork _db;
    public CategoryService(IUnitOfWork unitOfWork)
    {
        _db = unitOfWork;
    }

    public List<Category> Get()
    {
        return _db.Categories.GetAll().ToList();
    }

    public Category GetById(Guid id)
    {
        var res = _db.Categories.Include(x => x.Products).SingleOrDefault(x => x.Id == id);

        if (res == null)
        {
            throw new NotFoundException(nameof(id), "Category with this Id was not founded!");
        }

        return res;
    }

    public Category GetByName(string name)
    {
        var res = _db.Categories.Include(x => x.Products).SingleOrDefault(x => x.Name == name);

        if (res == null)
        {
            throw new NotFoundException(nameof(name), "Category with this name was not founded!");
        }

        return res;
    }

    public async Task<Category> Create(Category category)
    {
        if (_db.Categories.GetAll().SingleOrDefault(x => x.Name == category.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(category.Name), "Category with this name alredy exists!");
        }

        await _db.Categories.AddAsync(category);

        return category;
    }

    public async Task<Category> Update(Category category)
    {
        var res = _db.Categories.GetById(category.Id);

        if (res == null)
        {
            throw new NotFoundException(nameof(category.Id), "Category with this Id was not founded!");
        }
            
        else if ((res.Name != category.Name) &&
                _db.Categories.GetAll().SingleOrDefault(x => x.Name == category.Name) != null)
        {
            throw new ObjectNotUniqueException(nameof(category.Name), "Category with this name already exists!");
        }

        await _db.Categories.UpdateAsync(category);

        return category;
    }
}
