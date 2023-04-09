using AutoMapper;
using OrderAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models.DTO.Order;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;
using OrderAPI.DataBase.Entities;
using OrderAPI.Models.DTO.Cart;
using System.Net;

namespace OrderAPI.Controllers;

/// <summary>
/// Provides the APIs for handling all the order logic
/// </summary>
[ApiController]
[Authorize(Policy = "LimitedAccessToOrders")]
[Route("api/v1/OrderAPI/[controller]/[action]")]
public class OrderController : ControllerBase
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    private readonly IOrderService _orderService;

    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="OrderController"/>.
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="orderService"></param>
    /// <param name="mapper"></param>
    public OrderController(IOrderService orderService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _orderService = orderService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns> Action result of getting all orders </returns>
    /// <response code="200"> Successful completion </response>
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderListDTOResponse>), (int)HttpStatusCode.OK)]
    public IActionResult GetAll()
    {
        var order = _orderService.GetAll();
        var response = _mapper.Map<List<OrderListDTOResponse>>(order);

        return Ok(response);
    }

    /// <summary>
    /// Get order by Id
    /// </summary>
    /// <param name="id"> Order Id </param>
    /// <returns> Action result of getting order by Id </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Order with this Id wasn't founded </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(OrderDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public IActionResult GetById(Guid id)
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

    /// <summary>
    /// Get orders by filters
    /// </summary>
    /// <param name="filter"> Orders filters </param>
    /// <returns> Action result of getting order by filters </returns>
    /// <response code="200"> Successful completion </response>
    [HttpPost]
    [ProducesResponseType(typeof(List<OrderListDTOResponse>), (int)HttpStatusCode.OK)]
    public IActionResult GetByFilter(OrderFilterDTORequest filter)
    {
        var orders = _orderService.GetByFilter(filter);
        var response = _mapper.Map<OrderListDTOResponse>(orders);

        return Ok(response);
    }

    /// <summary>
    /// Create order
    /// </summary>
    /// <param name="model"> The order data transfer object as request </param>
    /// <returns> Task object containing the action result of creating order </returns>
    /// <response code="201"> Order created </response>
    /// <response code="401"> Order is empty </response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDTOResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(OrderDTORequest model)
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

    /// <summary>
    /// Update order data
    /// </summary>
    /// <param name="id"> Order Id </param>
    /// <param name="model"> The order data transfer object as request </param>
    /// <returns> Task object containing the action result of updating order data </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Order is empty </response>
    /// <response code="404"> Order with this Id wasn't founded </response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(OrderDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(Guid id, OrderDTORequest model)
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

    /// <summary>
    /// Order is ready
    /// </summary>
    /// <param name="id"> Order Id </param>
    /// <returns> Task object containing the action result </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Bad request </response>
    /// <response code="404"> Order with this Id wasn't founded </response>
    [HttpPatch("{id:Guid}")]
    [Authorize(Policy = "FullAccessToOrders")]
    [ProducesResponseType(typeof(OrderDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> IsReady(Guid id)
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

    /// <summary>
    /// Order is recieved
    /// </summary>
    /// <param name="id"> Order Id </param>
    /// <returns> Task object containing the action result </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Bad request </response>
    /// <response code="404"> Order with this Id wasn't founded </response>
    [HttpPatch("{id:Guid}")]
    [Authorize(Policy = "FullAccessToOrders")]
    [ProducesResponseType(typeof(OrderDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> IsReceived(Guid id)
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

    /// <summary>
    /// Cansel order
    /// </summary>
    /// <param name="id"> Order Id </param>
    /// <returns> Task object containing the action result of canseling order </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Bad request </response>
    /// <response code="404"> Order with this Id wasn't founded </response>
    [HttpPatch("{id:Guid}")]
    [ProducesResponseType(typeof(OrderDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Cancel(Guid id)
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

    /// <summary>
    /// Payment order
    /// </summary>
    /// <param name="id"> Order Id </param>
    /// <returns> Task object containing the action result of paymenting order </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Bad request </response>
    /// <response code="404"> Order with this Id wasn't founded </response>
    [HttpPatch("{id:Guid}")]
    [Authorize(Policy = "FullAccessToOrders")]
    [ProducesResponseType(typeof(OrderDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> IsPaymented(Guid id)
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
