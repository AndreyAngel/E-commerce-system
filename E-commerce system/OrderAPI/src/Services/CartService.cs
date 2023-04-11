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

/// <summary>
/// Сlass providing the APIs for managing cart product in a persistence store.
/// </summary>
public class CartService: ICartService
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _db;

    /// <summary>
    /// <see cref="IBusControl"/>.
    /// </summary>
    private readonly IBusControl _bus;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Interface for class providing the APIs for managing cart product in a persistence store.
    /// </summary>
    private readonly ICartProductService _cartProductService;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Creates an instance of the <see cref="CartProductService"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    /// <param name="bus"> <see cref="IBusControl"/> </param>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    /// <param name="cartProductService"> Interface for class providing the APIs for managing cart product in a persistence store </param>
    public CartService(IUnitOfWork unitOfWork, IBusControl bus, IMapper mapper, ICartProductService cartProductService)
    {
        _db = unitOfWork;
        _bus = bus;
        _mapper = mapper;
        _cartProductService = cartProductService;
    }

    ~CartService() => Dispose(false);

    /// <inheritdoc/>
    public async Task<CartDomainModel> GetById(Guid id)
    {
        ThrowIfDisposed();

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
    /// <inheritdoc/>
    public async Task Create(Guid id)
    {
        ThrowIfDisposed();
        Cart cart = new() { Id = id };

        await _db.Carts.AddAsync(cart);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<CartDomainModel> ComputeTotalValue(Guid id)
    {
        ThrowIfDisposed();
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

    /// <inheritdoc/>
    public async Task<CartDomainModel> Clear(Guid id)
    {
        ThrowIfDisposed();
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

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db.Dispose();
                _cartProductService.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    /// <summary>
    /// Checks the relevance of products and returns a new cart
    /// </summary>
    /// <param name="cart"> Cart objcet </param>
    /// <returns> Task containing cart object with actualy product </returns>
    private async Task<CartDomainModel> Check(Cart cart)
    {
        ThrowIfDisposed();
        ProductListDTORabbitMQ<Guid> productsId = new();

        foreach (CartProduct cartProduct in cart.CartProducts)
        {
            productsId.Products.Add(cartProduct.ProductId);
        }

        Uri uri = new("rabbitmq://localhost/checkProductsQueue");

        ProductListDTORabbitMQ<ProductDTORabbitMQ> response =
        await RabbitMQClient.Request<ProductListDTORabbitMQ<Guid>,
        ProductListDTORabbitMQ<ProductDTORabbitMQ>>(_bus, productsId, uri);

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
        for (int i = 0; i < indexes.Count; i++)
        {
            model.CartProducts.RemoveAt(indexes[i] - i);
            await _cartProductService.Delete(cart.CartProducts[indexes[i] - i].Id);
        }

        model.ComputeTotalValue();

        return model;
    }
}