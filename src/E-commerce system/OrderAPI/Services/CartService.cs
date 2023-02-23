using OrderAPI.Services.Interfaces;
using OrderAPI.Models;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Infrastructure;
using Infrastructure.DTO;
using Infrastructure.Exceptions;

namespace OrderAPI.Services;

public class CartService: ICartService
{
    private readonly Context _db;
    private readonly IBusControl _bus;
    public CartService(Context context, IBusControl bus)
    {
        _db = context;
        _bus = bus;
    }

    public async Task<Cart> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart id!");

        Cart? cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");

        Cart result = await Check(cart);

        _db.Carts.Update(result);
        await _db.SaveChangesAsync();

        return result;
    }

    // The cart is created automatically after user registration 
    public async Task<Cart> Create(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart id!");

        //todo: передача идентификатора не равного UserId

        Cart cart = new() { Id = id };

        await _db.Carts.AddAsync(cart);
        await _db.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> ComputeTotalValue(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart id!");

        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");

        cart.ComputeTotalValue();
        _db.Carts.Update(cart);
        await _db.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> Clear(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart id!");

        //todo: передача идентификатора не равного UserId

        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");

        cart.Clear();

        _db.Carts.Update(cart);
        await _db.SaveChangesAsync();

        return cart;
    }

    // Checks the relevance of products and returns a new cart
    public async Task<Cart> Check(Cart cart)
    {
        ProductListDTO<int> products = new ProductListDTO<int>();

        foreach (CartProduct cartProduct in cart.CartProducts)
        {
            products.Products.Add(cartProduct.ProductDTOId);
        }

        Uri uri = new("rabbitmq://localhost/checkProductsQueue");
        ProductListDTO<ProductDTO> response =
            await RabbitMQClient.Request<ProductListDTO<int>, ProductListDTO<ProductDTO>>(_bus, products, uri);

        // The order of the objects in the response matches the order in the request
        // Replacing old objects with current ones
        List<int> indexes = new();
        for (int i = 0; i < response.Products.Count; i++)
        {
            if (response.Products[i] == null)
            {
                indexes.Add(i);
                continue;
            }

            if (response.Products[i].Id == cart.CartProducts[i].ProductDTOId)
            {
                cart.CartProducts[i].Product = response.Products[i];
                cart.CartProducts[i].ComputeTotalValue();
            }
            else
                indexes.Add(i);
        }
        // Delete the rest
        for(int i = 0; i < indexes.Count; i++)
        {
            cart.CartProducts.RemoveAt(indexes[i] - i);
        }

        cart.ComputeTotalValue();

        return cart;
    }
}