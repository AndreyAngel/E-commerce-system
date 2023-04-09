using OrderAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using OrderAPI.UnitOfWork.Interfaces;
using OrderAPI.Models.DTO.Cart;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Exceptions;
using OrderAPI.Models;
using System.Net;

namespace OrderAPI.Controllers;

/// <summary>
/// Provides the APIs for handling all the cart logic
/// </summary>
[ApiController]
[Authorize (Policy = "Cart")]
[Route("api/v1/OrderAPI/[controller]/[action]")]
public class CartController : ControllerBase
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICartService _cartService;

    private readonly ICartProductService _productService;

    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="CartController"/>.
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="cartService"></param>
    /// <param name="productService"></param>
    /// <param name="mapper"></param>
    public CartController(IUnitOfWork unitOfWork, ICartService cartService,
                          ICartProductService productService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cartService = cartService;
        _productService = productService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get cart by cart Id
    /// </summary>
    /// <param name="cartId"> Cart Id </param>
    /// <returns> Task object containig the action result of getting cart </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Incorrect cart Id </response>
    /// <response code="404"> Cart with this Id wasn't founded </response>
    [HttpGet("{cartId:Guid}")]
    [ProducesResponseType(typeof(CartDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetByCartId(Guid cartId)
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
    }

    /// <summary>
    /// Add new product to cart. If product has already existed to cart, increase the number.
    /// </summary>
    /// <param name="model"> Cart product data transfer object as request </param>
    /// <returns> Task object containig the action result of adding product </returns>
    /// <response code="201"> Product added </response>
    /// <response code="401"> Bad request data </response>
    /// <response code="404"> Cart or product with this Id wasn't founded </response>
    [HttpPost]
    [ProducesResponseType(typeof(CartProductDTOResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AddProduct(CartProductDTORequest model)
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
        catch (CatalogApiException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Change of the product quantity in cart
    /// </summary>
    /// <param name="cartProductId"> Cart product Id </param>
    /// <param name="model"> Cart product data transfer object as request </param>
    /// <returns> Task object containig the action result of changing quantity of product in cart </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Bad request data </response>
    /// <response code="404"> Cart or product with this Id wasn't founded </response>
    [HttpPatch("{cartProductId:Guid}")]
    [ProducesResponseType(typeof(CartDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> QuantityChange(Guid cartProductId, CartProductDTORequest model)
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
        catch (CatalogApiException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete product from cart
    /// </summary>
    /// <param name="cartId"> Cart Id </param>
    /// <param name="cartProductId"> Cart product Id </param>
    /// <returns> Task object containig the action result of removing product from cart </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Bad request data </response>
    /// <response code="404"> Cart or product with this Id wasn't founded </response>
    [HttpDelete("{cartId:Guid},{cartProductId:Guid}")]
    [ProducesResponseType(typeof(CartDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid cartId, Guid cartProductId)
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
    }

    /// <summary>
    /// Clear of the cart
    /// </summary>
    /// <param name="cartId"> Cart Id </param>
    /// <returns> Task object containing the action result of clearing of the cart </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Incorrect cart Id </response>
    /// <response code="404"> Cart with this Id wasn't founded </response>
    [HttpDelete("{cartId:Guid}")]
    [ProducesResponseType(typeof(CartDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Clear(Guid cartId)
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
    }
}
