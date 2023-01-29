using OrderAPI.Services.Interfaces;
using OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderAPI.Services;

public class CartService: ICartService
{
    private readonly Context _db;
    public CartService(Context context)
    {
        _db = context;
    }

    public async Task<Cart> GetById(int id)
    {
        return await _db.Carts.SingleOrDefaultAsync(x => x.Id == id);
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
}
