using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderAPI.Services.Interfaces;

namespace OrderAPI.Services;

public class CartProductService: ICartProductService
{
    private readonly Context _db;
    public CartProductService(Context context)
    {
        _db = context;
    }

    public async Task<CartProduct> Create(CartProduct cartProduct)
    {
        if (_db.CartProducts.Any(x => x.ProductId == cartProduct.ProductId && x.CartId == cartProduct.CartId))
        {
            CartProduct product = await _db.CartProducts.Include(x => x.Product).SingleAsync(x => x.ProductId == cartProduct.ProductId
                                                                                                && x.CartId == cartProduct.CartId);
            product.Quantity += cartProduct.Quantity;
            product.ComputeTotalValue();

            return await Update(cartProduct);
        }

        cartProduct.ComputeTotalValue();
        await _db.CartProducts.AddAsync(cartProduct);
        await _db.SaveChangesAsync();

        return cartProduct;
    }

    public async Task<CartProduct> Update(CartProduct cartProduct)
    {
        _db.CartProducts.Update(cartProduct);
        await _db.SaveChangesAsync();

        return cartProduct;
    }

    public async Task Delete(int id)
    {
        CartProduct cartProduct = new CartProduct { Id = id };
        _db.Entry(cartProduct).State = EntityState.Deleted;
        await _db.SaveChangesAsync();
    }
}
