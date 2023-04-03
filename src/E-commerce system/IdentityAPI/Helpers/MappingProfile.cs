using AutoMapper;
using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.ViewModels;
using IdentityAPI.Models.ViewModels.Requests;
using IdentityAPI.Models.ViewModels.Responses;

namespace IdentityAPI.Helpers;

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
