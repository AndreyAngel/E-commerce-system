﻿using Microsoft.EntityFrameworkCore;
using Infrastructure.DTO;
using MassTransit;
using AutoMapper;
using OrderAPI.UseCases.Interfaces;
using OrderAPI.Domain.Repositories.Interfaces;
using OrderAPI.Domain.Models;
using OrderAPI.Domain.Entities;
using Infrastructure.Exceptions;
using Infrastructure;

namespace OrderAPI.UseCases.Implementation;

/// <summary>
/// Сlass providing the APIs for managing cart product in a persistence store.
/// </summary>
public class CartProductService: ICartProductService
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
    public CartProductService(IUnitOfWork unitOfWork,  IBusControl bus, IMapper mapper)
    {
        _db = unitOfWork;
        _bus = bus;
        _mapper = mapper;
    }

    ~CartProductService() => Dispose(false);

    /// <inheritdoc/>
    public async Task<CartProductDomainModel> Create(CartProductDomainModel cartProduct)
    {
        ThrowIfDisposed();
        var response = await GetProductFromCatalog(cartProduct.ProductId);

        // Сheck if there is already such a product in the cart
        // If there is, to change of quantity
        // If there isn't, to add a new cart product
        if (_db.CartProducts.GetAll().Any(x => x.ProductId == cartProduct.ProductId && x.CartId == cartProduct.CartId))
        {
            CartProduct productEntity = await _db.CartProducts.GetAll()
                .SingleAsync(x => x.ProductId == cartProduct.ProductId && x.CartId == cartProduct.CartId);

            var product = _mapper.Map<CartProductDomainModel>(productEntity);

            product.Quantity += cartProduct.Quantity;
            product.ComputeTotalValue(response.Price.Value);

            var updatedProduct = await Update(product);

            var res = _mapper.Map<CartProductDomainModel>(updatedProduct);
            res.Product = _mapper.Map<ProductDomainModel>(response);

            return res;
        }

        cartProduct.ComputeTotalValue(response.Price.Value);

        var cartProductEntity = _mapper.Map<CartProduct>(cartProduct);

        await _db.CartProducts.AddAsync(cartProductEntity);

        var model = _mapper.Map<CartProductDomainModel>(cartProductEntity);
        model.Product = _mapper.Map<ProductDomainModel>(response);

        return model;
    }

    /// <inheritdoc/>
    public async Task Delete(Guid id)
    {
        ThrowIfDisposed();
        var res = _db.CartProducts.GetById(id);

        if (res == null)
        {
            throw new NotFoundException("Cart product with this Id not founded!", nameof(res.Id));
        } 

        await _db.CartProducts.RemoveAsync(res);
    }

    /// <inheritdoc/>
    public async Task<CartProductDomainModel> Update(CartProductDomainModel cartProduct)
    {
        ThrowIfDisposed();

        if (_db.CartProducts.GetById(cartProduct.Id) == null)
        {
            throw new NotFoundException("Cart product with this Id not founded!", nameof(cartProduct.Id));
        }

        var response = await GetProductFromCatalog(cartProduct.ProductId);

        cartProduct.ComputeTotalValue(response.Price.Value);

        var cartProductEntity = _mapper.Map<CartProduct>(cartProduct);

        await _db.CartProducts.UpdateAsync(cartProductEntity);
        cartProduct.Id = cartProductEntity.Id;

        return cartProduct;
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

    /// <inheritdoc/>
    private async Task<ProductDTORabbitMQ> GetProductFromCatalog(Guid productId)
    {
        ThrowIfDisposed();

        ProductDTORabbitMQ productDTO = new() { Id = productId };
        Uri uri = new("rabbitmq://localhost/getProductQueue");
        var response = await RabbitMQClient.Request<ProductDTORabbitMQ, ProductDTORabbitMQ>(_bus, productDTO, uri);

        if (response.ErrorMessage != null)
        {
            throw new CatalogApiException(response.ErrorMessage, nameof(productId));
        }

        return response;
    }
}
