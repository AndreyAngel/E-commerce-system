using Microsoft.EntityFrameworkCore;
using OrderAPI.Services.Interfaces;
using OrderAPI.Exceptions;
using Infrastructure.DTO;
using MassTransit;
using AutoMapper;
using OrderAPI.UnitOfWork.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure;
using OrderAPI.DataBase.Entities;
using OrderAPI.Models;

namespace OrderAPI.Services;

public class CartProductService: ICartProductService
{
    private readonly IUnitOfWork _db;

    private readonly IBusControl _bus;

    private readonly IMapper _mapper;

    public CartProductService(IUnitOfWork unitOfWork,  IBusControl bus, IMapper mapper)
    {
        _db = unitOfWork;
        _bus = bus;
        _mapper = mapper;
    }

    public async Task<CartProductDomainModel> Create(CartProductDomainModel cartProduct)
    {
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



    public async Task Delete(Guid id)
    {
        var res = _db.CartProducts.GetById(id);

        if (res == null)
        {
            throw new NotFoundException("Cart product with this Id not founded!", nameof(res.Id));
        } 

        await _db.CartProducts.RemoveAsync(res);
    }

    public async Task<CartProductDomainModel> Update(CartProductDomainModel cartProduct)
    {

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

    private async Task<ProductDTORabbitMQ> GetProductFromCatalog(Guid productId)
    {
        ProductDTORabbitMQ productDTO = new() { Id = productId };
        Uri uri = new("rabbitmq://localhost/getProductQueue");
        var response = await RabbitMQClient.Request<ProductDTORabbitMQ, ProductDTORabbitMQ>(_bus, productDTO, uri);

        if (response.ErrorMessage != null)
        {
            throw new EmptyOrderException(response.ErrorMessage, nameof(productId));
        }

        return response;
    }
}
