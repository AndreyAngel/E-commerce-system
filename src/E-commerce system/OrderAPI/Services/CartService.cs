using OrderAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using OrderAPI;
using OrderAPI.DTO;
using OrderAPI.Exceptions;
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

    public async Task<CartViewModelResponse> GetById(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts.Where(x => x.OrderId == null))
                                  .AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");
        }

        CartViewModelResponse result = await Check(cart);

        cart = _mapper.Map<Cart>(result);

        await _db.Carts.UpdateAsync(cart);

        return result;
    }

    // The cart is created automatically after user registration 
    public async Task<CartViewModelResponse> Create(Guid id)
    {
        Cart cart = new() { Id = id };

        await _db.Carts.AddAsync(cart);
        await _db.SaveChangesAsync();

        CartViewModelResponse model = _mapper.Map<CartViewModelResponse>(cart);

        return model;
    }

    public async Task<CartViewModelResponse> ComputeTotalValue(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");
        }

        CartViewModelResponse model = _mapper.Map<CartViewModelResponse>(cart);

        model.ComputeTotalValue();
        cart.TotalValue = model.TotalValue;

        await _db.Carts.UpdateAsync(cart);

        return model;
    }

    public async Task<CartViewModelResponse> Clear(Guid id)
    {
        var cart = await _db.Carts.Include(x => x.CartProducts).SingleOrDefaultAsync(x => x.Id == id);

        if (cart == null)
        {
            throw new NotFoundException(nameof(id), "Cart with this id was not founded!");
        }

        cart.Clear();

        await _db.Carts.UpdateAsync(cart);

        CartViewModelResponse model = _mapper.Map<CartViewModelResponse>(cart);

        return model;
    }

    // Checks the relevance of products and returns a new cart
    public async Task<CartViewModelResponse> Check(Cart cart)
    {
        ProductListDTO<Guid> productsId = new();

        foreach (CartProduct cartProduct in cart.CartProducts)
        {
            productsId.Products.Add(cartProduct.ProductId);
        }

        Uri uri = new("rabbitmq://localhost/checkProductsQueue");
        ProductListDTO<ProductDTO> response =
            await RabbitMQClient.Request<ProductListDTO<Guid>, ProductListDTO<ProductDTO>>(_bus, productsId, uri);

        CartViewModelResponse model = _mapper.Map<CartViewModelResponse>(cart);

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