using AutoMapper;
using OrderAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models.DTO.Order;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;
using OrderAPI.DataBase.Entities;

namespace OrderAPI.Controllers;

[ApiController]
[Authorize(Policy = "LimitedAccessToOrders")]
[Route("api/v1/OrderAPI/[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IOrderService _orderService;

    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _orderService = orderService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult< List<OrderListDTOResponse> > GetAll()
    {
        var order = _orderService.GetAll();
        var response = _mapper.Map<OrderListDTOResponse>(order);

        return Ok(response);
    }

    [HttpGet("{id:Guid}")]
    public ActionResult<OrderDTOResponse> GetById(Guid id)
    {
        try
        {
            var order = _orderService.GetById(id);
            var response = _mapper.Map<OrderDTOResponse>(order);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult<List<OrderListDTOResponse>> GetByFilter(OrderFilterDTORequest filter)
    {
        var orders = _orderService.GetByFilter(filter);
        var response = _mapper.Map<OrderListDTOResponse>(orders);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult< OrderDTOResponse >> Create(OrderDTORequest model)
    {
        try
        {
            var order = _mapper.Map<Order>(model);

            var res = await _orderService.Create(order);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderDTOResponse>(res);

            return Created(new Uri($"https://localhost:7045/api/v1/ord/Order/GetById/{response.Id}"), response);
        }
        catch (EmptyOrderException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<OrderDTOResponse>> Update(Guid id, OrderDTORequest model)
    {
        try
        {
            var order = _mapper.Map<Order>(model);
            order.Id = id;

            var res = await _orderService.Update(order);
            var response = _mapper.Map<OrderDTOResponse>(res);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (EmptyOrderException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id:Guid}")]
    [Authorize(Policy = "FullAccessToOrders")]
    public async Task<ActionResult<OrderDTOResponse>> IsReady(Guid id)
    {
        try
        {
            var order = await _orderService.IsReady(id);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderDTOResponse>(order);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (OrderStatusException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id:Guid}")]
    [Authorize(Policy = "FullAccessToOrders")]
    public async Task<ActionResult<OrderDTOResponse>> IsReceived(Guid id)
    {
        try
        {
            var order = await _orderService.IsReceived(id);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderDTOResponse>(order);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (OrderStatusException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id:Guid}")]
    public async Task<ActionResult<OrderDTOResponse>> Cancel(Guid id)
    {
        try
        {
            var order = await _orderService.Cancel(id);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderDTOResponse>(order);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (OrderStatusException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id:int}")]
    [Authorize(Policy = "FullAccessToOrders")]
    public async Task<ActionResult<OrderDTOResponse>> IsPaymented(Guid id)
    {
        try
        {
            var order = await _orderService.IsPaymented(id);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderDTOResponse>(order);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (OrderStatusException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
