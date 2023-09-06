using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Entities;
using Infrastructure.DTO;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CatalogAPI.UseCases;

/// <summary>
/// Class for models mapping
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of <see cref="MappingProfile"/>.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Product, ProductDTORabbitMQ>();

        CreateMap<BrandDTORequest, Brand>();

        CreateMap<Brand, BrandDTOResponse>();

        CreateMap<CategoryDTORequest, Category>();

        CreateMap<Category, CategoryDTOResponse>();

        CreateMap<ProductDTORequest, Product>();

        CreateMap<Product, ProductDTOResponse>();

        CreateMap<Product, ProductListDTOResponse>();
    }
}
