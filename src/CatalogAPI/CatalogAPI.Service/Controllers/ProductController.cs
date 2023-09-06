using Microsoft.AspNetCore.Mvc;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using CatalogAPI.Contracts.DTO;
using MediatR;
using CatalogAPI.UseCases.GetProductDetails;
using CatalogAPI.UseCases.CreateProduct;
using CatalogAPI.UseCases.UpdateProduct;
using CatalogAPI.UseCases.GetProductList;
using CatalogAPI.UseCases.WithdrawFromSale;

namespace CatalogAPI.Controllers;

/// <summary>
/// Provides the APIs for handling all the category logic
/// </summary>
[Route("api/v1/CatalogAPI/[controller]/[action]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Creates an instance of the <see cref="ProductController"/>.
    /// </summary>
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get the all products information
    /// </summary>
    /// <returns> The action result of getting products information </returns>
    /// <response code="200"> Successful completion </response>
    [HttpGet()]
    [ProducesResponseType(typeof(IAsyncEnumerable<ProductListDTOResponse>), (int)HttpStatusCode.OK)]
    public ActionResult<IAsyncEnumerable<ProductListDTOResponse>> GetList(
        string? sort = null,
        string? searchString = null,
        [FromQuery] ProductFilterDTO? filters = null)
    {
        var result = _mediator.CreateStream(new GetProductListQuery(sort, searchString, filters));
        return Ok(result);
    }

    /// <summary>
    /// Get the product information by Id
    /// </summary>
    /// <param name="id"> Product Id </param>
    /// <returns> The action result of getting product information </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Product with this Id wasn't founded </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetProductDetailsQuery(id));
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get the product information by name
    /// </summary>
    /// <param name="name"> Product name </param>
    /// <returns> The action result of getting product information </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Product with this name wasn't founded </response>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetDetails(string name)
    {
        try
        {
            var result = await _mediator.Send(new GetProductDetailsQuery(name));
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Create a new product or put it back on sale
    /// </summary>
    /// <param name="request"> Product data transfer object </param>
    /// <returns> The task object containing the action result of creating a new product </returns>
    /// <response code="201"> Successful completion </response>
    /// <response code="409"> Product with this name already exists </response>
    /// <response code="404"> category or category with this id wasn't founded </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPost]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Create(ProductDTORequest request)
    {
        try
        {
            var result = await _mediator.Send(new CreateProductCommand(request));
            return Created(
                new Uri($"http://localhost:44389/api/v1/CatalogAPI/Product/GetDetails/{result.Id}"), result);
        }
        catch (ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Change product data
    /// </summary>
    /// <param name="id"> Product Id </param>
    /// <param name="request"> Product data transfer object </param>
    /// <returns> The task object containing the action result of changing product </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="409"> Product with this name already exists </response>
    /// <response code="404"> Product, category or brand with this Id wasn't founded </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(Guid id, ProductDTORequest request)
    {
        try
        {
            await _mediator.Send(new UpdateProductCommand(id, request));
            return NoContent();
        }
        catch (ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Withdraw from sale
    /// Product is withdrawn from sale.
    /// </summary>
    /// <param name="id"> Product Id </param>
    /// <returns> The task object containing the action result of delete product </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Product with this Id wasn't founded </response>
    [HttpPatch("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> WithdrawFromSale(Guid id)
    {
        try
        {
            await _mediator.Send(new WithdrawProductFromSaleCommand(id));
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}