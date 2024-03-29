﻿using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Exceptions;
using System.Net;
using OrderAPI.Domain.Repositories.Interfaces;
using OrderAPI.UseCases.Interfaces;
using OrderAPI.Contracts.DTO.Cart;
using OrderAPI.Domain.Models;

namespace OrderAPI.Service.Controllers;

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

    /// <summary>
    /// Class providing the APIs for managing cart in a persistence store.
    /// </summary>
    private readonly ICartService _cartService;

    /// <summary>
    /// Class providing the APIs for managing cart product in a persistence store.
    /// </summary>
    private readonly ICartProductService _productService;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="CartController"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    /// <param name="cartService"> Class providing the APIs for managing cart in a persistence store. </param>
    /// <param name="productService"> Class providing the APIs for managing cart product in a persistence store. </param>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    public CartController(IUnitOfWork unitOfWork, ICartService cartService,
                          ICartProductService productService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cartService = cartService;
        _productService = productService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get cart by user Id
    /// </summary>
    /// <param name="userId"> User Id </param>
    /// <returns> Task object containig the action result of getting cart </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="400"> Incorrect user Id </response>
    /// <response code="401"> Unauthorized </response>
    /// <response code="404"> Cart with this Id wasn't founded </response>
    [HttpGet("{userId:Guid}")]
    [ProducesResponseType(typeof(CartDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        try
        {
            var currentUserId = new Guid((string)HttpContext.Items.First(x => x.Key == "UserId").Value);

            // The user id must match the cart id
            if (currentUserId != userId)
            {
                return BadRequest("Incorrect cart Id");
            }

            CartDomainModel cart = await _cartService.GetById(userId);
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
    /// <response code="400"> Bad request data </response>
    /// <response code="401"> Unauthorized </response>
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

            return Created(new Uri($"https://localhost:44389/api/v1/OrderAPI/Cart/GetById/{product.CartId}"), response);
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
    /// <response code="400"> Bad request data </response>
    /// <response code="401"> Unauthorized </response>
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
    /// <response code="400"> Bad request data </response>
    /// <response code="401"> Unauthorized </response>
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
    /// <response code="400"> Incorrect cart Id </response>
    /// <response code="401"> Unauthorized </response>
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
