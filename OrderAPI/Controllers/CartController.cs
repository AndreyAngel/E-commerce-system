using OrderAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;

namespace OrderAPI.Controllers
{
    [Route("api/v1/ord/cart/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartProductService _productService;

        public CartController(ICartService cartService, ICartProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        [HttpGet]
        [Route("{id:int}")]
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
        [Route("{IdCart:int},{IdProduct:int}")]
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
