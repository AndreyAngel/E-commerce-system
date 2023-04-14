using AutoMapper;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.Models.DTO;

namespace DeliveryAPI.Helpers;

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
        CreateMap<DeliveryDTORequest, Delivery>();

        CreateMap<Delivery, DeliveryDTOResponse>();

        CreateMap<AddressDTORequest, Address>();
    }
}
