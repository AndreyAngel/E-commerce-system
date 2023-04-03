using OrderAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels.Order;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork.Interfaces;

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
            throw new NotFoundException(nameof(id), "Order with this Id wasn't fouded");
        }

        return res;
    }

    public List<Order> GetByFilter(OrderFilterViewModelRequest filter)
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
            // Изменить исключение
            throw new Exception(nameof(order));
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
                throw new NotFoundException("CartProduct.Id", "CartProduct with this Id wasn't founded");
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
            throw new NotFoundException(nameof(order.Id), "Order with this Id wasn't founded");
        }

        if (order.CartProducts.Count == 0)
        {
            // Изменить исключение
            throw new Exception(nameof(order));
        }

        await _db.Orders.UpdateAsync(order);
        return order;
    }

    public async Task<Order> IsReady(Guid id)
    {
        var order = _db.Orders.GetById(id);

        if (order == null)
        {
            throw new NotFoundException(nameof(id), "Order with this Id wasn't fouded");
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
            throw new NotFoundException(nameof(id), "Order with this Id wasn't fouded");
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
            throw new NotFoundException(nameof(id), "Order wth this Id wasn't fouded");
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
            throw new NotFoundException(nameof(id), "Order with this Id wasn't fouded");
        }

        order.IsPaymented = true;
        await _db.Orders.UpdateAsync(order);

        return order;
    }
}
