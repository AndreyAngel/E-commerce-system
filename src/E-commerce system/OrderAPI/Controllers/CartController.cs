using OrderAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models.ViewModels;
using OrderAPI.Models.DataBase;

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
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                CartViewModel cart = await _cartService.GetById(id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CartProduct cartProduct)
        {
            try
            {
                CartProductViewModel product = await _productService.Create(cartProduct);
                await _cartService.ComputeTotalValue(product.CartId);
                
                // todo: изменить статус код ответа

                return Ok(product);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{IdCart:int},{IdProduct:int}")]
        public async Task<IActionResult> DeleteProduct(int IdCart, int IdProduct)
        {
            try
            {
                await _productService.Delete(IdProduct);
                CartViewModel cart = await _cartService.ComputeTotalValue(IdCart);
                
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> QuantityChange(CartProductViewModel cartProduct)
        {
            try
            {
                await _productService.Update(cartProduct);
                CartViewModel cart = await _cartService.ComputeTotalValue(cartProduct.CartId);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{IdCart:int}")]
        public async Task<IActionResult> Clear(int IdCart)
        {
            try
            {
                var res = await _cartService.Clear(IdCart);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
