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
    /// <summary>
    /// Initializes a new instance of <see cref="MappingProfile"/>.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<RegisterViewModel, User>().ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<UserUpdateViewModelRequest, User>();

        CreateMap<User, UserViewModelResponse>();

        CreateMap<AddressViewModel, Address>();

        CreateMap<Address, AddressViewModel>();
    }
}
