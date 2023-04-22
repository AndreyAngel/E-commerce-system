using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.UnitOfWork.Interfaces;
using CatalogAPI.DataBase.Entities;

namespace CatalogAPI.Services;

/// <summary>
/// Class providing the APIs for managing category in a persistence store.
/// </summary>
public class CategoryService: ICategoryService
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _db;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Creates an instance of the <see cref="ProductService"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    public CategoryService(IUnitOfWork unitOfWork)
    {
        _db = unitOfWork;
    }

    ~CategoryService() => Dispose(false);

    /// <inheritdoc/>
    public List<Category> GetAll()
    {
        ThrowIfDisposed();

        return _db.Categories.GetAll();
    }

    /// <inheritdoc/>
    public Category GetById(Guid id)
    {
        ThrowIfDisposed();

        var category = _db.Categories.Include(x => x.Products).SingleOrDefault(x => x.Id == id);

        if (category == null)
        {
            throw new NotFoundException("Category with this Id was not founded!", nameof(id));
        }

        return category;
    }

    /// <inheritdoc/>
    public Category GetByName(string name)
    {
        ThrowIfDisposed();

        var category = _db.Categories.Include(x => x.Products).SingleOrDefault(x => x.Name == name);

        if (category == null)
        {
            throw new NotFoundException("Category with this name was not founded!", nameof(name));
        }

        return category;
    }

    /// <inheritdoc/>
    public async Task<Category> Create(Category category)
    {
        ThrowIfDisposed();

        if (_db.Categories.GetAll().SingleOrDefault(x => x.Name == category.Name) != null)
        {
            throw new ObjectNotUniqueException("category with this name alredy exists!", nameof(category.Name));
        }

        await _db.Categories.AddAsync(category);

        return category;
    }

    /// <inheritdoc/>
    public async Task<Category> Update(Category category)
    {
        ThrowIfDisposed();

        var res = _db.Categories.GetById(category.Id);

        if (res == null)
        {
            throw new NotFoundException("category with this Id was not founded!", nameof(category.Id));
        }
            
        else if ((res.Name != category.Name) &&
                _db.Categories.GetAll().SingleOrDefault(x => x.Name == category.Name) != null)
        {
            throw new ObjectNotUniqueException("category with this name already exists!", nameof(category.Name));
        }

        await _db.Categories.UpdateAsync(category);

        return category;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}
