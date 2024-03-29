﻿using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CatalogAPI.Domain.Repositories.Implementation;

/// <summary>
/// The product repository class containing methods for interaction with the database
/// </summary>
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="ProductRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    /// <param name="memoryCache"> Represents a local in-memory cache whose values are not serialized </param>
    public ProductRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
    { }
}
