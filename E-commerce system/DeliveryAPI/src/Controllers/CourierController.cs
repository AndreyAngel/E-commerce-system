using AutoMapper;
using DeliveryAPI.Models.DTO;
using DeliveryAPI.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAPI.Controllers;

[Route("api/v1/DeliveryAPI/[controller]/[action]")]
[ApiController]
public class CourierController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly ICourierService _courierService;

    public CourierController(IMapper mapper, ICourierService courierService)
    {
        _mapper = mapper;
        _courierService = courierService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _courierService.GetAll();
        var response = _mapper.Map<CourierListDTOResponse>(result);

        return Ok(response);
    }

    [HttpGet("{Id:Guid}")]
    public IActionResult GetById(Guid Id)
    {
        try
        {
            var result = _courierService.GetById(Id);
            var response = _mapper.Map<CourierDTOResponse>(result);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
