using OrderAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.DTO.Order;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;
using OrderAPI.DataBase.Entities;
using AutoMapper;
using Infrastructure.DTO;
using OrderAPI.Models.DTO;
using Infrastructure;
using MassTransit;

namespace OrderAPI.Services;

/// <summary>
/// Сlass providing the APIs for managing cart product in a persistence store.
/// </summary>
public class OrderService : IOrderService
{
    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _db;

    /// <summary>
    /// Configurations of application
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// <see cref="IBusControl"/>.
    /// </summary>
    private readonly IBusControl _bus;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Creates an instance of the <see cref="OrderService"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    /// <param name="configuration"> Configurations of application </param>
    /// <param name="bus"> <see cref="IBusControl"/>. </param>
    public OrderService(IUnitOfWork unitOfWork, IMapper mapper,
                        IConfiguration configuration, IBusControl bus)
    {
        _db = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
        _bus = bus;
    }

    ~OrderService() => Dispose(false);

    /// <inheritdoc/>
    public List<Order> GetAll()
    {
        ThrowIfDisposed();
        return _db.Orders.GetAll().ToList();
    }

    /// <inheritdoc/>
    public Order GetById(Guid id)
    {
        ThrowIfDisposed();
        var res = _db.Orders.Include(x => x.OrderProducts).AsNoTracking().SingleOrDefault(x => x.Id == id);

        if (res == null)
        {
            throw new NotFoundException("Order with this Id wasn't fouded", nameof(id));
        }

        return res;
    }

    /// <inheritdoc/>
    public List<Order> GetByFilter(OrderFilterDTORequest filter)
    {
        ThrowIfDisposed();
        var orders = _db.Orders.GetAll();

        if (filter.UserId != null)
        {
            orders = orders.Where(x => x.UserId == filter.UserId);
        }

        if (filter.IsReady != null)
        {
            orders = orders.Where(x => x.IsReady == filter.IsReady);
        }

        if (filter.IsReceived != null)
        {
            orders = orders.Where(x => x.IsReceived == filter.IsReceived);
        }

        if (filter.IsCanceled != null)
        {
            orders = orders.Where(x => x.IsCanceled == filter.IsCanceled);
        }

        return orders.ToList();
    }

    /// <inheritdoc/>
    public async Task<Order> Create(Order order)
    {
        ThrowIfDisposed();
        if (order.OrderProducts.Count == 0)
        {
            throw new EmptyOrderException("Empty order", nameof(order));
        }

        for (int i = 0; i < order.OrderProducts.Count; i++)
        {
            var cartProduct = _db.CartProducts.GetById(order.OrderProducts[i].Id);

            if (cartProduct == null)
            {
                throw new NotFoundException("CartProduct with this Id wasn't founded", nameof(cartProduct.Id));
            }

            if (cartProduct.Id == order.OrderProducts[i].Id)
            {
                throw new ObjectNotUniqueException("Order with cart product Id has already exists", nameof(cartProduct.Id));
            }

            var orderProduct = _mapper.Map<OrderProduct>(cartProduct);
            order.OrderProducts[i] = orderProduct;
            order.TotalValue += orderProduct.TotalValue;
            await _db.CartProducts.RemoveAsync(cartProduct);
        }

        await _db.Orders.AddAsync(order);

        return order;
    }

    /// <inheritdoc/>
    public async Task<Order> Update(Order order)
    {
        ThrowIfDisposed();
        if (_db.Orders.GetById(order.Id) == null)
        {
            throw new NotFoundException("Order with this Id wasn't founded", nameof(order.Id));
        }

        if (order.OrderProducts.Count == 0)
        {
            throw new EmptyOrderException("Empty order", nameof(order));
        }

        await _db.Orders.UpdateAsync(order);
        return order;
    }

    /// <inheritdoc/>
    public async Task<Order> IsReady(Guid id)
    {
        ThrowIfDisposed();
        var order = _db.Orders.GetById(id);

        if (order == null)
        {
            throw new NotFoundException("Order with this Id wasn't fouded", nameof(id));
        }

        if (order.IsCanceled)
        {
            throw new OrderStatusException("Order has been canceled");
        }

        if (order.IsReceived)
        {
            throw new OrderStatusException("Order has already been received");
        }

        if (order.IsReady)
        {
            throw new OrderStatusException("Order has already been readied");
        }

        order.IsReady = true;
        await _db.Orders.UpdateAsync(order);

        return order;
    }

    /// <inheritdoc/>
    public async Task<Order> IsReceived(Guid id)
    {
        ThrowIfDisposed();
        var order = _db.Orders.GetById(id);

        if (order == null)
        {
            throw new NotFoundException("Order with this Id wasn't fouded", nameof(id));
        }

        if (order.IsReceived)
        {
            throw new OrderStatusException("Order has already been received");
        }

        if (order.IsCanceled)
        {
            throw new OrderStatusException("Order has been canceled");
        }

        if (!order.IsPaymented)
        {
            throw new OrderStatusException("Order hasn't yet been paid");
        }

        order.IsReceived = true;
        await _db.Orders.UpdateAsync(order);

        return order;
    }

    /// <inheritdoc/>
    public async Task<Order> Cancel(Guid id)
    {
        ThrowIfDisposed();
        var order = _db.Orders.GetById(id);

        if (order == null)
        {
            throw new NotFoundException("Order wth this Id wasn't fouded", nameof(id));
        }

        if (order.IsReceived)
        {
            throw new OrderStatusException("Order has already been received");
        }

        if (order.IsCanceled)
        {
            throw new OrderStatusException("Order has alredy been canceled");
        }

        order.IsCanceled = true;
        await _db.Orders.UpdateAsync(order);

        await CancelDelivery(order.Id);

        return order;
    }

    /// <inheritdoc/>
    public async Task<Order> IsPaymented(Guid id)
    {
        ThrowIfDisposed();
        var order = _db.Orders.GetById(id);

        if (order == null)
        {
            throw new NotFoundException("Order with this Id wasn't fouded", nameof(id));
        }

        if (!order.IsPaymented)
        {
            throw new OrderStatusException("Order has already been paid");
        }

        if (order.IsCanceled)
        {
            throw new OrderStatusException("Order has been canceled");
        }

        order.IsPaymented = true;
        await _db.Orders.UpdateAsync(order);

        return order;
    }

    /// <inheritdoc/>
    public async Task CreateDelivery(Order order, AddressDTO address)
    {
        ThrowIfDisposed();

        var deliveryDTO = new DeliveryDTORabbitMQ()
        {
            OrderId = order.Id,
            Address = _mapper.Map<AddressDTORabbitMQ>(address)
        };

        await RabbitMQClient.Request(_bus, deliveryDTO,
            new($"{_configuration["RabbitMQ:Host"]}/createDeliveryQueue"));
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    private async Task CancelDelivery(Guid orderId)
    {
        ThrowIfDisposed();

        await RabbitMQClient.Request(_bus, new CancelDeliveryDTORabbitMQ() { OrderId = orderId },
            new($"{_configuration["RabbitMQ:Host"]}/cancelDeliveryQueue"));
    }
}
