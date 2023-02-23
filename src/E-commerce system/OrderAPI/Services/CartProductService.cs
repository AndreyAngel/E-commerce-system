using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderAPI.Services.Interfaces;
using Infrastructure.Exceptions;

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
        if (cartProduct.Id != 0)
            cartProduct.Id = 0;

        //todo: new Exception - передача идентификатора не своей корзины

        if (_db.CartProducts.Any(x => x.ProductId == cartProduct.ProductId && x.CartId == cartProduct.CartId))
        {
            CartProduct product = await _db.CartProducts.Include(x => x.Product).SingleAsync(x => x.ProductId == cartProduct.ProductId
                                                                                                && x.CartId == cartProduct.CartId);
            product.Quantity += cartProduct.Quantity;
            product.ComputeTotalValue();

            return await Update(product);
        }

        cartProduct.ComputeTotalValue();
        await _db.CartProducts.AddAsync(cartProduct);
        await _db.SaveChangesAsync();

        return cartProduct;
    }

    public async Task<CartProduct> Update(CartProduct cartProduct)
    {
        if (cartProduct.Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(cartProduct.Id), "Invalid cart product Id");

        //todo: new Exception - передача идентификатора не своей корзины

        if (await _db.CartProducts.SingleOrDefaultAsync(x => x.Id == cartProduct.Id) == null)
            throw new NotFoundException(nameof(cartProduct.Id), "Cart product with this Id not founded!");

        cartProduct.ComputeTotalValue();
        _db.CartProducts.Update(cartProduct);
        await _db.SaveChangesAsync();

        return cartProduct;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart product Id");

        var res = await _db.CartProducts.SingleOrDefaultAsync(x => x.Id == id);

        if (res == null)
            throw new NotFoundException(nameof(res.Id), "Cart product with this Id not founded!");

        _db.CartProducts.Remove(res);
        await _db.SaveChangesAsync();
    }
}
