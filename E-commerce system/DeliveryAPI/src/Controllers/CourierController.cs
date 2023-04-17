using AutoMapper;
using DeliveryAPI.Models.DTO;
using DeliveryAPI.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAPI.Controllers;

/// <summary>
/// Provides the APIs for handling all the courier logic
/// </summary>
[Route("api/v1/DeliveryAPI/[controller]/[action]")]
[ApiController]
public class CourierController : ControllerBase
{
    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Object of class <see cref="ICourierService"/> providing the APIs for managing courier data in a persistence store.
    /// </summary>
    private readonly ICourierService _courierService;

    /// <summary>
    /// Creates an instance of the <see cref="CourierController"/>.
    /// </summary>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    /// <param name="courierService"> Object of class <see cref="ICourierService"/> providing the APIs
    /// for managing courier data in a persistence store. </param>
    public CourierController(IMapper mapper, ICourierService courierService)
    {
        _mapper = mapper;
        _courierService = courierService;
    }

    /// <summary>
    /// Get information about all couriers
    /// </summary>
    /// <returns> Action result of getting of information about all couriers </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Unauthorized </response>
    [HttpGet]
    [Authorize(Policy = "Get information about the courier")]
    [ProducesResponseType(typeof(CourierDTOResponse), (int)HttpStatusCode.OK)]
    public IActionResult GetAll()
    {
        var result = _courierService.GetAll();
        var response = _mapper.Map<List<CourierDTOResponse>>(result);

        return Ok(response);
    }

    /// <summary>
    /// Get information about single courier by Id
    /// </summary>
    /// <param name="Id"> Courier Id </param>
    /// <returns> Action result of getting of information about single courier </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Unauthorized </response>
    /// <response code="404"> Courier with this Id wasn't founded </response>
    [HttpGet("{Id:Guid}")]
    [Authorize(Policy = "Get information about the courier")]
    [ProducesResponseType(typeof(CourierDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
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
