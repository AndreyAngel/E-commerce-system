using AutoMapper;
using DeliveryAPI.Contracts.DTO;
using DeliveryAPI.Domain.Repositories.Interfaces;
using DeliveryAPI.UseCases.Interfaces;
using DeliveryAPI.UseCases.Interfaces.Exceptions;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAPI.Service.Controllers;

/// <summary>
/// Provides the APIs for handling all the delivery logic
/// </summary>
[Route("api/v1/DeliveryAPI/[controller]/[action]")]
[ApiController]
public class DeliveryController : ControllerBase
{
    /// <summary>
    /// Object of class <see cref="IDeliveryService"/> providing the APIs for managing delivery in a persistence store.
    /// </summary>
    private readonly IDeliveryService _deliveryService;

    /// <summary>
    /// A repository group interface providing a common data context
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="DeliveryController"/>.
    /// </summary>
    /// <param name="unitOfWork"> A repository group interface providing a common data context </param>
    /// <param name="deliveryService"> Object of class <see cref="IDeliveryService"/>
    /// providing the APIs for managing delivery in a persistence store </param>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    public DeliveryController(IDeliveryService deliveryService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _deliveryService = deliveryService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all deliveries
    /// </summary>
    /// <returns> The action result of getting of deliveries </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Unauthorized </response>
    [HttpGet]
    [Authorize(Policy = "Get information by delivery")]
    [ProducesResponseType(typeof(DeliveryDTOResponse), (int)HttpStatusCode.OK)]
    public IActionResult GetAll()
    {
        var result = _deliveryService.GetAll();
        var response = _mapper.Map<List<DeliveryDTOResponse>>(result);

        return Ok(response);
    }

    /// <summary>
    /// Get of delivery by Id
    /// </summary>
    /// <param name="Id"> Delivery Id </param>
    /// <returns> The action result of getting of delivery </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Unauthorized </response>
    /// <response code="404"> Delivery with this Id wasn't founded </response>
    [HttpGet("{Id:Guid}")]
    [Authorize(Policy = "Get information by delivery")]
    [ProducesResponseType(typeof(DeliveryDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public IActionResult GetById(Guid Id)
    {
        try
        {
            var result = _deliveryService.GetById(Id);
            var response = _mapper.Map<DeliveryDTOResponse>(result);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get of delivery by filters
    /// </summary>
    /// <param name="filters"> Filters </param>
    /// <returns> The action result of getting of deliveries </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Unauthorized </response>
    [HttpGet]
    [Authorize(Policy = "Get information by delivery")]
    [ProducesResponseType(typeof(DeliveryDTOResponse), (int)HttpStatusCode.OK)]
    public IActionResult GetByFilter(DeliveryFilterDTORequest filters)
    {
        var result = _deliveryService.GetByFilter(filters);
        var response = _mapper.Map<List<DeliveryDTOResponse>>(result);

        return Ok(response);
    }

    /// <summary>
    /// Pick up of the order from warehouse by courier
    /// </summary>
    /// <param name="orderId"> Order Id </param>
    /// <returns> Task object containing action result </returns>
    /// <response code="204"> Successful completion </response>
    /// <response code="400"> Action cannot be performed </response>
    /// <response code="401"> Unauthorized </response>
    /// <response code="404"> Delivery with this order Id wasn't founded </response>
    [HttpPatch("{orderId:Guid}")]
    [Authorize(Policy = "Courier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> PickUpOrderFromWarehouse(Guid orderId)
    {
        try
        {
            var courierId = HttpContext.Items["UserId"].ToString();
            _deliveryService.PickUpOrderFromWarehouse(orderId, Guid.Parse(courierId));
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        catch (DeliveryStatusException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Complete of delivery
    /// </summary>
    /// <param name="Id"> Delivery Id </param>
    /// <returns> Task object containing action result of completing of delivery </returns>
    /// <response code="204"> Successful completion </response>
    /// <response code="400"> Action cannot be performed </response>
    /// <response code="401"> Unauthorized </response>
    /// <response code="404"> Delivery with this Id wasn't founded </response>
    [HttpPatch("{Id:Guid}")]
    [Authorize(Policy = "Courier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Complete(Guid Id)
    {
        try
        {
            _deliveryService.Complete(Id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        catch (DeliveryStatusException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Return of the order to warehouse by courier
    /// </summary>
    /// <param name="orderId"> Order Id </param>
    /// <returns> Task object containing action result </returns>
    /// <response code="204"> Successful completion </response>
    /// <response code="400"> Action cannot be performed </response>
    /// <response code="401"> Unauthorized </response>
    /// <response code="404"> Delivery with this order Id wasn't founded </response>
    [HttpPatch("{orderId:Guid}")]
    [Authorize(Policy = "Courier")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ReturnToWarehouse(Guid orderId)
    {
        try
        {
            _deliveryService.ReturnToWarehouse(orderId);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        catch (DeliveryStatusException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
