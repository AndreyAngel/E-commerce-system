﻿using CatalogAPI.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CatalogAPI.UnitOfWork;

/// <summary>
/// The brand repository class containing methods for interaction with the database
/// </summary>
public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="BrandRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    /// <param name="memoryCache"> Represents a local in-memory cache whose values are not serialized </param>
    public BrandRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
    { }
}
