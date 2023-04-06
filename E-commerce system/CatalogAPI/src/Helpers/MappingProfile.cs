using AutoMapper;
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

        CreateMap<Brand, BrandDTOResponce>();

        CreateMap<CategoryDTORequest, Category>();

        CreateMap<Category, CategoryDTOResponce>();

        CreateMap<ProductDTORequest, Product>();

        CreateMap<Product, ProductDTOResponce>();

        CreateMap<Product, ProductListDTOResponce>();
    }
}
