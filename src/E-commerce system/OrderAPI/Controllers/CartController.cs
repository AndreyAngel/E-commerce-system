using OrderAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models.DataBase;
using AutoMapper;
using Infrastructure.Exceptions;
using OrderAPI.UnitOfWork.Interfaces;
using OrderAPI.Models.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Authorize (Policy = "Cart")]
    [Route("api/v1/ord/[controller]/[action]")]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly ICartProductService _productService;
        private readonly IMapper _mapper;

        public CartController(IUnitOfWork unitOfWork, ICartService cartService,
                              ICartProductService productService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("{cartId:Guid}")]
        public async Task<ActionResult<CartViewModelResponse>> GetById(Guid cartId)
        {
            try
            {
                CartViewModelResponse cart = await _cartService.GetById(cartId);
                return Ok(cart);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(CatalogApiException ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartProductViewModelResponse>> AddProduct(CartProductViewModelRequest model)
        {
            try
            {
                CartProduct cartProduct = _mapper.Map<CartProduct>(model);
                CartProductViewModelResponse product = await _productService.Create(cartProduct);
                await _cartService.ComputeTotalValue(cartProduct.CartId);

                await _unitOfWork.SaveChangesAsync();

                return Created(new Uri($"https://localhost:7045/api/v1/ord/Cart/GetById/{cartProduct.CartId}"), product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CatalogApiException ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        [HttpDelete("{cartId:Guid},{cartProductId:Guid}")]
        public async Task<ActionResult<CartViewModelResponse>> DeleteProduct(Guid cartId, Guid cartProductId)
        {
            try
            {
                await _productService.Delete(cartProductId);
                CartViewModelResponse cart = await _cartService.ComputeTotalValue(cartId);

                await _unitOfWork.SaveChangesAsync();
                
                return Ok(cart);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        [HttpPut("{cartProductId:Guid}")]
        public async Task<ActionResult<CartViewModelResponse>> QuantityChange(Guid cartProductId, CartProductViewModelRequest model)
        {
            try
            {
                CartProduct product = _mapper.Map<CartProduct>(model);
                product.Id = cartProductId;

                await _productService.Update(product);
                CartViewModelResponse cart = await _cartService.ComputeTotalValue(model.CartId);

                await _unitOfWork.SaveChangesAsync();

                return Ok(cart);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CatalogApiException ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        [HttpDelete("{cartId:Guid}")]
        public async Task<ActionResult<CartViewModelResponse>> Clear(Guid cartId)
        {
            try
            {
                var res = await _cartService.Clear(cartId);
                await _unitOfWork.SaveChangesAsync();

                return Ok(res);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
