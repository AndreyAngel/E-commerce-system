using OrderAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models.ViewModels;
using OrderAPI.Models.DataBase;
using AutoMapper;
using Infrastructure.Exceptions;

namespace OrderAPI.Controllers
{
    [Route("api/v1/ord/cart/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartProductService _productService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, ICartProductService productService, IMapper mapper)
        {
            _cartService = cartService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{cartId:int}")]
        public async Task<ActionResult<CartViewModel>> GetById(int cartId)
        {
            try
            {
                CartViewModel cart = await _cartService.GetById(cartId);
                return Ok(cart);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(CatalogApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
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

                return Created(new Uri($"https://localhost:7045/api/v1/ord/cart/GetById/{model.CartId}"), product);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CatalogApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{cartId:int},{cartProductId:int}")]
        public async Task<ActionResult<CartViewModel>> DeleteProduct(int cartId, int cartProductId)
        {
            try
            {
                await _productService.Delete(cartProductId);
                CartViewModel cart = await _cartService.ComputeTotalValue(cartId);
                
                return Ok(cart);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{cartProductId:int}")]
        public async Task<ActionResult<CartViewModel>> QuantityChange(int cartProductId, CartProductViewModelRequest model)
        {
            try
            {
                CartProduct product = _mapper.Map<CartProduct>(model);
                product.Id = cartProductId;

                await _productService.Update(product);
                CartViewModel cart = await _cartService.ComputeTotalValue(model.CartId);

                return Ok(cart);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CatalogApiException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{cartId:int}")]
        public async Task<ActionResult<CartViewModel>> Clear(int cartId)
        {
            try
            {
                var res = await _cartService.Clear(cartId);
                return Ok(res);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
