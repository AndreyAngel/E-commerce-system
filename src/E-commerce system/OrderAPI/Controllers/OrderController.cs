using AutoMapper;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels.Order;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork.Interfaces;

namespace OrderAPI.Controllers;

[Route("api/v1/ord/order/[action]")]
[ApiController]
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

    [HttpGet("{id:int}")]
    public ActionResult<OrderViewModelResponse> GetById(int id)
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

            return Created(new Uri($"https://localhost:7045/api/v1/ord/order/GetById/{response.Id}"), response);
        }
        //Сделать Exception
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<OrderViewModelResponse>> Update(int id, OrderViewModelRequest model)
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
        //Сделать Exception
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<OrderViewModelResponse>> IsReady(int id)
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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<OrderViewModelResponse>> IsReceived(int id)
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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<OrderViewModelResponse>> IsCanceled(int id)
    {
        try
        {
            var order = await _orderService.IsCanceled(id);
            await _unitOfWork.SaveChangesAsync();

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

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<OrderViewModelResponse>> IsPaymented(int id)
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
        finally
        {
            _unitOfWork.Dispose();
        }
    }
}
