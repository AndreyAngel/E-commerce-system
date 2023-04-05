using Microsoft.EntityFrameworkCore;
using OrderAPI.Services.Interfaces;
using OrderAPI.Exceptions;
using OrderAPI.DTO;
using MassTransit;
using OrderAPI.Models.DataBase;
using AutoMapper;
using OrderAPI.UnitOfWork.Interfaces;
using OrderAPI.Models.ViewModels.Cart;

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

    public async Task<CartProductViewModelResponse> Create(CartProduct cartProduct)
    {
        var response = await GetProductFromCatalog(cartProduct.ProductId);

        // Сheck if there is already such a product in the cart
        // If there is, to change of quantity
        // If there isn't, to add a new cart product
        if (_db.CartProducts.GetAll().Any(x => x.ProductId == cartProduct.ProductId && x.CartId == cartProduct.CartId))
        {
            CartProduct product = await _db.CartProducts.GetAll()
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

        var model = _mapper.Map<CartProductViewModelResponse>(cartProduct);
        model.Product = _mapper.Map<ProductViewModel>(response);

        return model;
    }

    public async Task<CartProduct> Update(CartProduct cartProduct)
    {

        if (_db.CartProducts.GetById(cartProduct.Id) == null)
        {
            throw new NotFoundException("Cart product with this Id not founded!", nameof(cartProduct.Id));
        }

        var response = await GetProductFromCatalog(cartProduct.ProductId);

        cartProduct.ComputeTotalValue(response.Price.Value);

        await _db.CartProducts.UpdateAsync(cartProduct);

        return cartProduct;
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

    private async Task<ProductDTO> GetProductFromCatalog(Guid productId)
    {
        ProductDTO productDTO = new() { Id = productId };
        Uri uri = new("rabbitmq://localhost/getProductQueue");
        var response = await RabbitMQClient.Request<ProductDTO, ProductDTO>(_bus, productDTO, uri);

        if (response.ErrorMessage != null)
        {
            throw new EmptyOrderException(response.ErrorMessage, nameof(productId));
        }

        return response;
    }
}
