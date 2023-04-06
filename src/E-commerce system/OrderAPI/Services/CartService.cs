using OrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Infrastructure.DTO;
using Infrastructure.Exceptions;
using AutoMapper;
using OrderAPI.UnitOfWork.Interfaces;
using Infrastructure;
using OrderAPI.DataBase.Entities;
using OrderAPI.Models;

namespace OrderAPI.Services;

public class CartService: ICartService
{
    private readonly IUnitOfWork _db;
    private readonly IBusControl _bus;
    private readonly IMapper _mapper;
    private readonly ICartProductService _cartProductService;

    public CartService(IUnitOfWork unitOfWork, IBusControl bus, IMapper mapper, ICartProductService cartProductService)
    {
        _db = unitOfWork;
        _bus = bus;
        _mapper = mapper;
        _cartProductService = cartProductService;
    }

    public async Task<CartDomainModel> GetById(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts)
                                  .AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException("Cart with this id was not founded!", nameof(id));
        }

        CartDomainModel result = await Check(cart);

        cart = _mapper.Map<Cart>(result);

        await _db.Carts.UpdateAsync(cart);

        return result;
    }

    // The cart is created after user registration 
    public async Task Create(Guid id)
    {
        Cart cart = new() { Id = id };

        await _db.Carts.AddAsync(cart);
        await _db.SaveChangesAsync();
    }

    public async Task<CartDomainModel> ComputeTotalValue(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException("Cart with this id was not founded!", nameof(id));
        }

        CartDomainModel model = _mapper.Map<CartDomainModel>(cart);

        model.ComputeTotalValue();
        cart.TotalValue = model.TotalValue;

        await _db.Carts.UpdateAsync(cart);

        return model;
    }

    public async Task<CartDomainModel> Clear(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException("Cart with this id was not founded!", nameof(id));
        }

        CartDomainModel model = _mapper.Map<CartDomainModel>(cart);
        model.Clear();
        cart.TotalValue = model.TotalValue;

        await _db.Carts.UpdateAsync(cart);

        return model;
    }

    // Checks the relevance of products and returns a new cart
    private async Task<CartDomainModel> Check(Cart cart)
    {
        ProductListDTORabbitMQ<Guid> productsId = new();

        foreach (CartProduct cartProduct in cart.CartProducts)
        {
            productsId.Products.Add(cartProduct.ProductId);
        }

        Uri uri = new("rabbitmq://localhost/checkProductsQueue");
        ProductListDTORabbitMQ<ProductDTORabbitMQ> response =
        await RabbitMQClient.Request<ProductListDTORabbitMQ<Guid>, ProductListDTORabbitMQ<ProductDTORabbitMQ>>(_bus, productsId, uri);

        CartDomainModel model = _mapper.Map<CartDomainModel>(cart);

        // The order of the objects in the response matches the order in the request
        // Replacing old objects with current ones
        List<int> indexes = new();
        for (int i = 0; i < response.Products.Count; i++)
        {
            if (response.Products[i] == null)
            {
                indexes.Add(i);
            }
            else if (response.Products[i].Id == model.CartProducts[i].ProductId)
            {
                var product = _mapper.Map<ProductDomainModel>(response.Products[i]);
                model.CartProducts[i].Product = product;
                model.CartProducts[i].ComputeTotalValue();
            }
            else
                indexes.Add(i);
        }
        // Delete the rest
        for(int i = 0; i < indexes.Count; i++)
        {
            model.CartProducts.RemoveAt(indexes[i] - i);
            await _cartProductService.Delete(cart.CartProducts[indexes[i] - i].Id);
        }

        model.ComputeTotalValue();
        
        return model;
    }
}