using OrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Infrastructure;
using Infrastructure.DTO;
using Infrastructure.Exceptions;
using OrderAPI.Models.DataBase;
using AutoMapper;
using OrderAPI.UnitOfWork.Interfaces;
using OrderAPI.Models.ViewModels.Cart;

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

    public async Task<CartViewModel> GetById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart id!");
        }

        var cart = await _db.Carts.Include(x => x.CartProducts.Where(x => x.OrderId == null))
                                  .AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");
        }

        CartViewModel result = await Check(cart);

        cart = _mapper.Map<Cart>(result);

        await _db.Carts.UpdateAsync(cart);

        return result;
    }

    // The cart is created automatically after user registration 
    public async Task<CartViewModel> Create(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart id!");
        }

        //todo: передача идентификатора не равного UserId

        Cart cart = new() { Id = id };

        await _db.Carts.AddAsync(cart);

        CartViewModel model = _mapper.Map<CartViewModel>(cart);

        return model;
    }

    public async Task<CartViewModel> ComputeTotalValue(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart id!");
        }
            
        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");
        }

        CartViewModel model = _mapper.Map<CartViewModel>(cart);

        model.ComputeTotalValue();
        cart.TotalValue = model.TotalValue;

        await _db.Carts.UpdateAsync(cart);

        return model;
    }

    public async Task<CartViewModel> Clear(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart id!");
        } 

        //todo: передача идентификатора не равного UserId

        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");
        }

        cart.Clear();

        await _db.Carts.UpdateAsync(cart);

        CartViewModel model = _mapper.Map<CartViewModel>(cart);

        return model;
    }

    // Checks the relevance of products and returns a new cart
    public async Task<CartViewModel> Check(Cart cart)
    {
        ProductListDTO<int> productsId = new ProductListDTO<int>();

        foreach (CartProduct cartProduct in cart.CartProducts)
        {
            productsId.Products.Add(cartProduct.ProductId);
        }

        Uri uri = new("rabbitmq://localhost/checkProductsQueue");
        ProductListDTO<ProductDTO> response =
            await RabbitMQClient.Request<ProductListDTO<int>, ProductListDTO<ProductDTO>>(_bus, productsId, uri);

        CartViewModel model = _mapper.Map<CartViewModel>(cart);

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
                var product = _mapper.Map<ProductViewModel>(response.Products[i]);
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