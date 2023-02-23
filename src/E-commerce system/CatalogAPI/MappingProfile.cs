using AutoMapper;
using CatalogAPI.Models;

namespace CatalogAPI;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Product, Infrastructure.Models.Product>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));
    }
}
