using AutoMapper;
using OrderAPI.Models.DataBase.Entities;
using OrderAPI.Models.ViewModels;
using OrderAPI.Models.ViewModels.Requests;
using OrderAPI.Models.ViewModels.Responses;

namespace OrderAPI.Helpers;

/// <summary>
/// Class for models mapping
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterViewModel, User>().ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<UserUpdateViewModelRequest, User>();

        CreateMap<User, UserViewModelResponse>();

        CreateMap<AddressViewModel, Address>();

        CreateMap<Address, AddressViewModel>();
    }
}
