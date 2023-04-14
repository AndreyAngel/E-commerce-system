using CatalogAPI.DataBase.Entities;
using Infrastructure.Exceptions;

namespace CatalogAPI.Services.Interfaces;

/// <summary>
/// Interface for class providing the APIs for managing category in a persistence store.
/// </summary>
public interface ICategoryService : IDisposable
{
    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns> Categories list, <see cref="List{Category}"/> </returns>
    public List<Category> GetAll();

    /// <summary>
    /// Get category by Id
    /// </summary>
    /// <param name="id"> category Id </param>
    /// <returns> <see cref="Category"/> </returns>
    /// <exception cref="NotFoundException"> Category with this Id wasn't founded </exception>
    public Category GetById(Guid id);

    /// <summary>
    /// Get category by name
    /// </summary>
    /// <param name="name"> Category name </param>
    /// <returns> <see cref="Category"/> </returns>
    /// <exception cref="NotFoundException"> Category with this name wasn't founded </exception>
    public Category GetByName(string name);

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="category"> New category </param>
    /// <returns> Task containing a created category, <see cref="Category"/> </returns>
    /// <exception cref="ObjectNotUniqueException"> Category with this name alredy exists </exception>
    public Task<Category> Create(Category category);

    /// <summary>
    /// Change category data
    /// </summary>
    /// <param name="category"> category data for changing </param>
    /// <returns> Task containing a chenged category, <see cref="Category"/> </returns>
    /// <exception cref="NotFoundException"> Category with this name wasn't founded </exception>
    /// <exception cref="ObjectNotUniqueException"> Category with this name alredy exists </exception>
    public Task<Category> Update(Category category);
}
