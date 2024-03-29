﻿using AutoMapper;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.Models.DTO;
using IdentityAPI.Models.DTO.Requests;
using IdentityAPI.Models.DTO.Response;
using Infrastructure.DTO;

namespace IdentityAPI.Helpers;

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
        CreateMap<RegisterDTORequest, User>().ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<RegisterCourierDTORequest, User>().ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<UserDTORequest, User>();

        CreateMap<User, UserDTOResponse>();

        CreateMap<AddressDTO, Address>();

        CreateMap<Address, AddressDTO>();

        CreateMap<User, CourierDTORabbitMQ>();
    }
}
