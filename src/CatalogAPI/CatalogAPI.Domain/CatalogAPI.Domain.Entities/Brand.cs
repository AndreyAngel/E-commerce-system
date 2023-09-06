﻿using CatalogAPI.Domain.Entities.Abstractions;

namespace CatalogAPI.Domain.Entities;

/// <summary>
/// Stores category data
/// </summary>
public class Brand : BaseEntity
{
    /// <summary>
    /// Gets or sets a product list in this category
    /// </summary>
    public virtual List<Product> Products { get; set; } = new();

    public Brand(string name, string? description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreationDate = DateTime.Now;
    }
}
