using AutoMapper;
using DeliveryAPI.Contracts.DTO;
using DeliveryAPI.Domain.Entities;
using Infrastructure.DTO;

namespace DeliveryAPI.Service.Helpers;

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

        CreateMap<AddressDTO, Address>();

        CreateMap<CourierDTORabbitMQ, Courier>();

        CreateMap<DeliveryDTORabbitMQ,  Delivery>();

        CreateMap<Courier, CourierDTOResponse>();
    }
}
