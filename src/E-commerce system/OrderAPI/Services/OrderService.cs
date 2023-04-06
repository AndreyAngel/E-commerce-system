using OrderAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.DTO.Order;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;
using OrderAPI.DataBase.Entities;

namespace OrderAPI.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _db;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _db = unitOfWork;
    }

    public List<Order> GetAll()
    {
        return _db.Orders.GetAll().ToList();
    }

    public Order GetById(Guid id)
    {
        var res = _db.Orders.Include(x => x.CartProducts).AsNoTracking().SingleOrDefault(x => x.Id == id);

        if (res == null)
        {
            throw new NotFoundException("Order with this Id wasn't fouded", nameof(id));
        }

        return res;
    }

    public List<Order> GetByFilter(OrderFilterDTORequest filter)
    {
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

    public async Task<Order> Create(Order order)
    {
        if (order.CartProducts.Count == 0)
        {
            throw new EmptyOrderException("Empty order", nameof(order));
        }

        List<CartProduct> cartProducts = new();
        for (int i = 0; i < order.CartProducts.Count; i++)
        {
            cartProducts.Add(new CartProduct() { Id = order.CartProducts[i].Id });
        }

        await _db.Orders.AddAsync(order);

        for (int i = 0; i < cartProducts.Count; i++)
        {
            var cartProduct = _db.CartProducts.GetById(cartProducts[i].Id);

            if (cartProduct == null)
            {
                throw new NotFoundException("CartProduct with this Id wasn't founded", "CartProduct.Id");
            }

            cartProduct.OrderId = order.Id;
            await _db.CartProducts.UpdateAsync(cartProduct);
            order.CartProducts.Add(cartProduct);
        }

        return order;
    }

    public async Task<Order> Update(Order order)
    {
        if (_db.Orders.GetById(order.Id) == null)
        {
            throw new NotFoundException("Order with this Id wasn't founded", nameof(order.Id));
        }

        if (order.CartProducts.Count == 0)
        {
            throw new EmptyOrderException("Empty order", nameof(order));
        }

        await _db.Orders.UpdateAsync(order);
        return order;
    }

    public async Task<Order> IsReady(Guid id)
    {
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

    public async Task<Order> IsReceived(Guid id)
    {
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

    public async Task<Order> Cancel(Guid id)
    {
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

        return order;
    }

    public async Task<Order> IsPaymented(Guid id)
    {
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
}
