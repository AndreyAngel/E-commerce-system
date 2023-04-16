using AutoMapper;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.Models.DTO;
using DeliveryAPI.Services;
using DeliveryAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Exceptions;

namespace DeliveryAPI.Controllers;

[Route("api/v1/DeliveryAPI/[controller]/[action]")]
[ApiController]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public DeliveryController(IDeliveryService deliveryService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _deliveryService = deliveryService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all deliveries
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _deliveryService.GetAll();
        var response = _mapper.Map<List<DeliveryDTOResponse>>(result);

        return Ok(response);
    }

    [HttpGet("{Id:Guid}")]
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

    [HttpPost]
    public async Task<IActionResult> Create(DeliveryDTORequest model)
    {
        try
        {
            await _deliveryService.ConfirmOrderId(model.OrderId);

            var delivery = _mapper.Map<Delivery>(model);
            var result = await _deliveryService.Create(delivery);
            var response = _mapper.Map<DeliveryDTOResponse>(result);

            await _unitOfWork.SaveChangesAsync();

            return Created(new Uri($"https://localhost:44389/api/v1/DeliveryAPI/Delivery/GetById/{response.Id}"), response);
        }
        catch (ObjectNotUniqueException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{orderId:Guid}")]
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
    }

    [HttpPatch("{Id:Guid}")]
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
    }

    [HttpPatch("{Id:Guid}")]
    public async Task<IActionResult> ReturnToWarehouse(Guid Id)
    {
        try
        {
            _deliveryService.ReturnToWarehouse(Id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        catch (DeliveryStatusException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
