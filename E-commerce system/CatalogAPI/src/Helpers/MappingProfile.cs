﻿using AutoMapper;
using CatalogAPI.Models.DataBase;
using CatalogAPI.Models.DTO;
using Infrastructure.DTO;

namespace CatalogAPI.Helpers;

public class MappingProfile : Profile
{
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
