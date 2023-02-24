using Microsoft.EntityFrameworkCore;
using OrderAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.DTO;
using Infrastructure;
using MassTransit;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels;
using AutoMapper;

namespace OrderAPI.Services;

public class CartProductService: ICartProductService
{
    private readonly Context _db;
    private readonly IBusControl _bus;
    private readonly IMapper _mapper;
    public CartProductService(Context context, IBusControl bus, IMapper mapper)
    {
        _db = context;
        _bus = bus;
        _mapper = mapper;
    }

    public async Task<CartProductViewModel> Create(CartProduct cartProduct)
    {
        if (cartProduct.Id != 0)
            cartProduct.Id = 0;

        CartProductViewModel model = _mapper.Map<CartProductViewModel>(cartProduct);

        // Getting of product by ID from Catalog service
        ProductDTO productDTO = new() { Id = model.ProductId };
        Uri uri = new("rabbitmq://localhost/getProductQueue");
        ProductDTO response = await RabbitMQClient.Request<ProductDTO, ProductDTO>(_bus, productDTO, uri);

        if (response.ErrorMessage != null)
            throw new CatalogApiException(nameof(cartProduct.ProductId), response.ErrorMessage);

        model.Product = response;

        //todo: new Exception - передача идентификатора не своей корзины


        // проверка на наличие уже такого товара в карзине
        if (_db.CartProducts.Any(x => x.ProductId == model.ProductId && x.CartId == model.CartId))
        {
            CartProduct product = await _db.CartProducts
                .SingleAsync(x => x.ProductId == model.ProductId && x.CartId == model.CartId);

            model.Quantity += product.Quantity;
            model.Id = product.Id;
            model.ComputeTotalValue();

            return await Update(model);
        }

        model.ComputeTotalValue();

        var result = _mapper.Map<CartProduct>(model);

        await _db.CartProducts.AddAsync(result);
        await _db.SaveChangesAsync();

        return model;
    }

    public async Task<CartProductViewModel> Update(CartProductViewModel cartProduct)
    {
        if (cartProduct.Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(cartProduct.Id), "Invalid cart product Id");

        //todo: new Exception - передача идентификатора не своей корзины

        if (await _db.CartProducts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == cartProduct.Id) == null)
            throw new NotFoundException(nameof(cartProduct.Id), "Cart product with this Id not founded!");

        // Getting of product by ID from Catalog service
        ProductDTO productDTO = new() { Id = cartProduct.ProductId };
        Uri uri = new("rabbitmq://localhost/getProductQueue");
        ProductDTO response = await RabbitMQClient.Request<ProductDTO, ProductDTO>(_bus, productDTO, uri);

        if (response.ErrorMessage != null)
            throw new CatalogApiException(nameof(cartProduct.ProductId), response.ErrorMessage);

        cartProduct.Product = response;
        cartProduct.ComputeTotalValue();

        var res = _mapper.Map<CartProduct>(cartProduct);

        _db.CartProducts.Update(res);
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
