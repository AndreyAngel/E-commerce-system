using AutoMapper;
using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.ViewModels;

namespace IdentityAPI;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterViewModel, User>()
            .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<UserUpdateViewModelRequest, User>();

        CreateMap<User, UserViewModelResponse>();

        CreateMap<AddressViewModel, Address>();

        CreateMap<Address, AddressViewModel>();
    }
}
