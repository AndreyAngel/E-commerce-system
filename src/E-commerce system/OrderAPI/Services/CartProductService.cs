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

    public async Task<CartProductViewModelResponse> Create(CartProduct cartProduct)
    {
        if (cartProduct.Id != 0)
            cartProduct.Id = 0;

        // Getting of product by ID from Catalog service
        ProductDTO productDTO = new() { Id = cartProduct.ProductId };
        Uri uri = new("rabbitmq://localhost/getProductQueue");
        ProductDTO response = await RabbitMQClient.Request<ProductDTO, ProductDTO>(_bus, productDTO, uri);

        if (response.ErrorMessage != null)
        {
            throw new CatalogApiException(nameof(cartProduct.ProductId), response.ErrorMessage);
        }

        //todo: new Exception - передача идентификатора не своей корзины


        // проверка на наличие уже такого товара в карзине
        if (_db.CartProducts.Any(x => x.ProductId == cartProduct.ProductId && x.CartId == cartProduct.CartId))
        {
            CartProduct product = await _db.CartProducts
                .SingleAsync(x => x.ProductId == cartProduct.ProductId && x.CartId == cartProduct.CartId);

            product.Quantity += cartProduct.Quantity;
            product.ComputeTotalValue(response.Price.Value);

            await Update(product);

            var res = _mapper.Map<CartProductViewModelResponse>(product);
            res.Product = _mapper.Map<ProductViewModel>(response);

            return res;
        }

        cartProduct.ComputeTotalValue(response.Price.Value);

        await _db.CartProducts.AddAsync(cartProduct);
        await _db.SaveChangesAsync();

        var model = _mapper.Map<CartProductViewModelResponse>(cartProduct);
        model.Product = _mapper.Map<ProductViewModel>(response);

        return model;
    }

    public async Task<CartProduct> Update(CartProduct cartProduct)
    {
        if (cartProduct.Id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cartProduct.Id), "Invalid cart product Id");
        }

        //todo: new Exception - передача идентификатора не своей корзины

        if (await _db.CartProducts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == cartProduct.Id) == null)
        {
            throw new NotFoundException(nameof(cartProduct.Id), "Cart product with this Id not founded!");
        }

        // Getting of product by ID from Catalog service
        ProductDTO productDTO = new() { Id = cartProduct.ProductId };
        Uri uri = new("rabbitmq://localhost/getProductQueue");
        ProductDTO response = await RabbitMQClient.Request<ProductDTO, ProductDTO>(_bus, productDTO, uri);

        if (response.ErrorMessage != null)
        {
            throw new CatalogApiException(nameof(cartProduct.ProductId), response.ErrorMessage);
        } 

        cartProduct.ComputeTotalValue(response.Price.Value);

        _db.CartProducts.Update(cartProduct);
        await _db.SaveChangesAsync();

        return cartProduct;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Invalid cart product Id");
        }

        var res = await _db.CartProducts.SingleOrDefaultAsync(x => x.Id == id);

        if (res == null)
        {
            throw new NotFoundException(nameof(res.Id), "Cart product with this Id not founded!");
        } 

        _db.CartProducts.Remove(res);
        await _db.SaveChangesAsync();
    }
}
