using CatalogAPI.Models.DataBase;
using Infrastructure.Exceptions;

namespace CatalogAPI.Services.Interfaces;

/// <summary>
/// Interface for class providing the APIs for managing category in a persistence store.
/// </summary>
public interface IBrandService : IDisposable
{
    /// <summary>
    /// Get all brands
    /// </summary>
    /// <returns> Brands list, <see cref="List{category}"/> </returns>
    public List<Brand> GetAll();

    /// <summary>
    /// Get category by Id
    /// </summary>
    /// <param name="id"> category Id </param>
    /// <returns> <see cref="Brand"/> </returns>
    /// <exception cref="NotFoundException"> category with this Id wasn't founded </exception>
    public Brand GetById(Guid id);

    /// <summary>
    /// Get category by name
    /// </summary>
    /// <param name="name"> category name </param>
    /// <returns> <see cref="Brand"/> </returns>
    /// <exception cref="NotFoundException"> category with this name wasn't founded </exception>
    public Brand GetByName(string name);

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="category"> New category </param>
    /// <returns> Task containing a created category, <see cref="Brand"/> </returns>
    /// <exception cref="ObjectNotUniqueException"> category with this name alredy exists </exception>
    public Task<Brand> Create(Brand category);

    /// <summary>
    /// Change category data
    /// </summary>
    /// <param name="category"> category data for changing </param>
    /// <returns> Task containing a chenged category, <see cref="Brand"/> </returns>
    /// <exception cref="NotFoundException"> category with this name wasn't founded </exception>
    /// <exception cref="ObjectNotUniqueException"> category with this name alredy exists </exception>
    public Task<Brand> Update(Brand category);
}

