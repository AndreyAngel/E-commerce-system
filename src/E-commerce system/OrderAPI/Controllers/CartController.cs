using OrderAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using OrderAPI.Exceptions;
using OrderAPI.UnitOfWork.Interfaces;
using OrderAPI.Models.DTO.Cart;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Exceptions;
using OrderAPI.Models;

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
                var userId = new Guid((string?)HttpContext.Items.First(x => x.Key == "UserId").Value);

                // The user id must match the cart id
                if (userId != cartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                CartDomainModel cart = await _cartService.GetById(cartId);
                var response = _mapper.Map<CartDTOResponse>(cart);

                return Ok(response);
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
                var userId = new Guid((string?)HttpContext.Items.First(x => x.Key == "UserId").Value);

                // The user id must match the cart id
                if (userId != model.CartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                CartProductDomainModel cartProduct = _mapper.Map<CartProductDomainModel>(model);
                CartProductDomainModel product = await _productService.Create(cartProduct);

                await _cartService.ComputeTotalValue(cartProduct.CartId);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<CartProductDTOResponse>(product);

                return Created(new Uri($"https://localhost:7045/api/v1/ord/Cart/GetById/{product.CartId}"), response);
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

        [HttpPatch("{cartProductId:Guid}")]
        public async Task<ActionResult<CartDTOResponse>> QuantityChange(Guid cartProductId,
                                                                              CartProductDTORequest model)
        {
            try
            {
                var userId = new Guid((string?)HttpContext.Items.First(x => x.Key == "UserId").Value);

                // The user id must match the cart id
                if (userId != model.CartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                CartProductDomainModel product = _mapper.Map<CartProductDomainModel>(model);
                product.Id = cartProductId;

                await _productService.Update(product);
                CartDomainModel cart = await _cartService.ComputeTotalValue(model.CartId);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<CartDTOResponse>(cart);

                return Ok(response);
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
                var userId = new Guid((string?)HttpContext.Items.First(x => x.Key == "UserId").Value);

                // The user id must match the cart id
                if (userId != cartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                await _productService.Delete(cartProductId);
                CartDomainModel cart = await _cartService.ComputeTotalValue(cartId);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<CartDTOResponse>(cart);

                return Ok(response);
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

        [HttpDelete("{cartId:Guid}")]
        public async Task<ActionResult<CartDTOResponse>> Clear(Guid cartId)
        {
            try
            {
                var userId = new Guid((string?)HttpContext.Items.First(x => x.Key == "UserId").Value);

                // The user id must match the cart id
                if (userId != cartId)
                {
                    return BadRequest("Incorrect cart Id");
                }

                var cart = await _cartService.Clear(cartId);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<CartDTOResponse>(cart);

                return Ok(response);
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
