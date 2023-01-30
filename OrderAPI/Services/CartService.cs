using OrderAPI.Services.Interfaces;
using OrderAPI.Models;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Infrastructure.DTO;

namespace OrderAPI.Services;

public class CartService: ICartService
{
    private readonly Context _db;
    private readonly IBus _bus;
    public CartService(Context context, IBus bus)
    {
        _db = context;
        _bus = bus;
    }

    public async Task<Cart> GetById(int id)
    {
        Cart? cart = await _db.Carts.Include(x => x.cartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
            throw new Exception("Cart not found!");

        Cart result = await Check(cart);
        return result;
    }

    public async Task<Cart> Create(Cart cart)
    {
        await _db.Carts.AddAsync(cart);
        await _db.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> ComputeTotalValue(int id)
    {
        Cart? cart = await _db.Carts.Include(x => x.cartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
            throw new Exception("Cart not found!");

        cart.ComputeTotalValue();
        _db.Carts.Update(cart);
        await _db.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> Clear(int id)
    {
        Cart? cart = await _db.Carts.Include(x => x.cartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
            throw new Exception("Cart not found!");

        cart.Clear();

        _db.Carts.Update(cart);
        await _db.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> Check(Cart cart)
    {
        ProductList products = new ProductList();

        foreach(CartProduct cartProduct in cart.cartProducts)
        {
            products.Products.Add(cartProduct.Product);
        }

        Uri uri = new Uri("rebbitmq/localhost/checkProductsQueue");
        var EndPoint = await _bus.GetSendEndpoint(uri);
        await EndPoint.Send(products);

        return new Cart();

        // TODO
    }
}
