using OrderAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using OrderAPI.Exceptions;
using OrderAPI.UnitOfWork.Interfaces;
using OrderAPI.Models.DTO.Cart;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Exceptions;
using OrderAPI.DataBase.Entities;

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
        public async Task<ActionResult<CartDTOResponse>> GetById(Guid cartId)
        {
            try
            {
                var userId = (Guid?)HttpContext.Items["UserId"];

                // The user id must match the cart id
                if (userId != cartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                CartDTOResponse cart = await _cartService.GetById(cartId);

                return Ok(cart);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(EmptyOrderException ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartProductDTOResponse>> AddProduct(CartProductDTORequest model)
        {
            try
            {
                var userId = (Guid?)HttpContext.Items["UserId"];

                // The user id must match the cart id
                if (userId != model.CartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                CartProduct cartProduct = _mapper.Map<CartProduct>(model);
                CartProductDTOResponse product = await _productService.Create(cartProduct);
                await _cartService.ComputeTotalValue(cartProduct.CartId);

                await _unitOfWork.SaveChangesAsync();

                return Created(new Uri($"https://localhost:7045/api/v1/ord/Cart/GetById/{cartProduct.CartId}"), product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (EmptyOrderException ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        [HttpDelete("{cartId:Guid},{cartProductId:Guid}")]
        public async Task<ActionResult<CartDTOResponse>> DeleteProduct(Guid cartId, Guid cartProductId)
        {
            try
            {
                var userId = (Guid?)HttpContext.Items["UserId"];

                // The user id must match the cart id
                if (userId != cartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                await _productService.Delete(cartProductId);
                CartDTOResponse cart = await _cartService.ComputeTotalValue(cartId);

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
        public async Task<ActionResult<CartDTOResponse>> QuantityChange(Guid cartProductId,
                                                                              CartProductDTORequest model)
        {
            try
            {
                var userId = (Guid?)HttpContext.Items["UserId"];

                // The user id must match the cart id
                if (userId != model.CartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                CartProduct product = _mapper.Map<CartProduct>(model);
                product.Id = cartProductId;

                await _productService.Update(product);
                CartDTOResponse cart = await _cartService.ComputeTotalValue(model.CartId);

                await _unitOfWork.SaveChangesAsync();

                return Ok(cart);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (EmptyOrderException ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        [HttpDelete("{cartId:Guid}")]
        public async Task<ActionResult<CartDTOResponse>> Clear(Guid cartId)
        {
            try
            {
                var userId = (Guid?)HttpContext.Items["UserId"];

                // The user id must match the cart id
                if (userId != cartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

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
