using OrderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using MassTransit;

namespace OrderAPI.Controllers
{
    [Route("api/v1/ord/cart/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly CartProductService _productService;

        public CartController(CartService cartService, CartProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> GetById(int id)
        {
            try
            {
                Cart cart = await _cartService.GetById(id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> Create(Cart cart)
        {
            try
            {
                Cart res = await _cartService.Create(cart);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> AddProduct(CartProduct cartProduct)
        {
            try
            {
                CartProduct product = await _productService.Create(cartProduct);
                Cart cart = await _cartService.ComputeTotalValue(product.CartId);

                return Ok(cart);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Cart>> DeleteProduct(int IdCart, int IdProduct)
        {
            try
            {
                await _productService.Delete(IdProduct);
                Cart cart = await _cartService.ComputeTotalValue(IdCart);
                
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Cart>> QuantityChange(CartProduct cartProduct)
        {
            try
            {
                await _productService.Update(cartProduct);
                Cart cart = await _cartService.ComputeTotalValue(cartProduct.CartId);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
