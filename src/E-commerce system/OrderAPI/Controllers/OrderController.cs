using AutoMapper;
using OrderAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels.Order;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.Controllers;

[ApiController]
[Authorize(Policy = "LimitedAccessToOrders")]
[Route("api/v1/ord/[controller]/[action]")]
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
    public ActionResult< List<OrderListViewModelResponse> > GetAll()
    {
        try
        {
            var order = _orderService.GetAll();
            var response = _mapper.Map<OrderListViewModelResponse>(order);

            return Ok(response);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpGet("{id:Guid}")]
    public ActionResult<OrderViewModelResponse> GetById(Guid id)
    {
        try
        {
            var order = _orderService.GetById(id);
            var response = _mapper.Map<OrderViewModelResponse>(order);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPost]
    public ActionResult<List<OrderListViewModelResponse>> GetByFilter(OrderFilterViewModelRequest filter)
    {
        try
        {
            var orders = _orderService.GetByFilter(filter);
            var response = _mapper.Map<OrderListViewModelResponse>(orders);

            return Ok(response);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPost]
    public async Task<ActionResult< OrderViewModelResponse >> Create(OrderViewModelRequest model)
    {
        try
        {
            var order = _mapper.Map<Order>(model);

            var res = await _orderService.Create(order);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderViewModelResponse>(res);

            return Created(new Uri($"https://localhost:7045/api/v1/ord/Order/GetById/{response.Id}"), response);
        }
        catch (EmptyOrderException ex)
        {
            return BadRequest(ex.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<OrderViewModelResponse>> Update(Guid id, OrderViewModelRequest model)
    {
        try
        {
            var order = _mapper.Map<Order>(model);
            order.Id = id;

            var res = await _orderService.Update(order);
            var response = _mapper.Map<OrderViewModelResponse>(res);

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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPatch("{id:Guid}")]
    [Authorize(Policy = "FullAccessToOrders")]
    public async Task<ActionResult<OrderViewModelResponse>> IsReady(Guid id)
    {
        try
        {
            var order = await _orderService.IsReady(id);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderViewModelResponse>(order);

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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPatch("{id:Guid}")]
    [Authorize(Policy = "FullAccessToOrders")]
    public async Task<ActionResult<OrderViewModelResponse>> IsReceived(Guid id)
    {
        try
        {
            var order = await _orderService.IsReceived(id);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderViewModelResponse>(order);

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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPatch("{id:Guid}")]
    public async Task<ActionResult<OrderViewModelResponse>> Cancel(Guid id)
    {
        try
        {
            var order = await _orderService.Cancel(id);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderViewModelResponse>(order);

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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPatch("{id:int}")]
    [Authorize(Policy = "FullAccessToOrders")]
    public async Task<ActionResult<OrderViewModelResponse>> IsPaymented(Guid id)
    {
        try
        {
            var order = await _orderService.IsPaymented(id);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<OrderViewModelResponse>(order);

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
        finally
        {
            _unitOfWork.Dispose();
        }
    }
}
