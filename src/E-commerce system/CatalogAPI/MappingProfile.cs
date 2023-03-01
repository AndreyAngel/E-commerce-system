using AutoMapper;
using CatalogAPI.Models.DataBase;
using CatalogAPI.Models.ViewModels;
using Infrastructure.DTO;

namespace CatalogAPI;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>();

        CreateMap<BrandViewModelRequest, Brand>();

        CreateMap<Brand, BrandViewModelResponce>();

        CreateMap<CategoryViewModelRequest, Category>();

        CreateMap<Category, CategoryViewModelResponce>();

        CreateMap<ProductViewModelRequest, Product>();

        CreateMap<Product, ProductViewModelResponce>();

        CreateMap<Product, ProductListViewModelResponce>();
    }
}
