using OrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Infrastructure.DTO;
using Infrastructure.Exceptions;
using AutoMapper;
using OrderAPI.UnitOfWork.Interfaces;
using OrderAPI.Models.DTO.Cart;
using Infrastructure;
using OrderAPI.DataBase.Entities;

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

    public async Task<CartDTOResponse> GetById(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts.Where(x => x.OrderId == null))
                                  .AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException("Cart with this id was not founded!", nameof(id));
        }

        CartDTOResponse result = await Check(cart);

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

    public async Task<CartDTOResponse> ComputeTotalValue(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException("Cart with this id was not founded!", nameof(id));
        }

        CartDTOResponse model = _mapper.Map<CartDTOResponse>(cart);

        model.ComputeTotalValue();
        cart.TotalValue = model.TotalValue;

        await _db.Carts.UpdateAsync(cart);

        return model;
    }

    public async Task<CartDTOResponse> Clear(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException("Cart with this id was not founded!", nameof(id));
        }

        cart.Clear();

        await _db.Carts.UpdateAsync(cart);

        CartDTOResponse model = _mapper.Map<CartDTOResponse>(cart);

        return model;
    }

    // Checks the relevance of products and returns a new cart
    private async Task<CartDTOResponse> Check(Cart cart)
    {
        ProductListDTORabbitMQ<Guid> productsId = new();

        foreach (CartProduct cartProduct in cart.CartProducts)
        {
            productsId.Products.Add(cartProduct.ProductId);
        }

        Uri uri = new("rabbitmq://localhost/checkProductsQueue");
        ProductListDTORabbitMQ<Infrastructure.DTO.ProductDTORabbitMQ> response =
            await RabbitMQClient.Request<ProductListDTORabbitMQ<Guid>, ProductListDTORabbitMQ<Infrastructure.DTO.ProductDTORabbitMQ>>(_bus, productsId, uri);

        CartDTOResponse model = _mapper.Map<CartDTOResponse>(cart);

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
                var product = _mapper.Map<Models.DTO.Cart.ProductDTO>(response.Products[i]);
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