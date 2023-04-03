﻿using AutoMapper;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels;
using OrderAPI.DTO;

namespace OrderAPI.Helpers;

public class MappingProfile : Profile
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
