using Microsoft.AspNetCore.Mvc;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using CatalogAPI.Contracts.DTO;
using MediatR;
using CatalogAPI.UseCases.GetBrandsList;
using CatalogAPI.UseCases.GetBrandDetails;
using CatalogAPI.UseCases.CreateBrand;
using CatalogAPI.UseCases.UpdateBrand;

namespace CatalogAPI.Controllers;


/// <summary>
/// Provides the APIs for handling all the category logic
/// </summary>
[Route("api/v1/CatalogAPI/[controller]/[action]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Creates an instance of the <see cref="BrandController"/>.
    /// </summary>
    public BrandController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get the all brands information
    /// </summary>
    /// <returns> The action result of getting brands information </returns>
    /// <response code="200"> Successful completion </response>
    [HttpGet()]
    [ProducesResponseType(typeof(IAsyncEnumerable<BrandDTOResponse>), (int)HttpStatusCode.OK)]
    public ActionResult<IAsyncEnumerable<BrandDTOResponse>> GetList(string? sort = null, string? searchString = null)
    {
        var result = _mediator.CreateStream(new GetBrandListQuery(sort, searchString));
        return Ok(result);
    }

    /// <summary>
    /// Get the brand information by Id
    /// </summary>
    /// <param name="id"> Brand Id </param>
    /// <returns> The action result of getting brand information </returns>
    /// <response code="200"> Successful completion </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(BrandDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetBrandDetailsQuery(id));
            return Ok(result);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get the brand information by name
    /// </summary>
    /// <param name="name"> Brand name </param>
    /// <returns> The action result of getting brand information </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Brand with this name wasn't founded </response>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(BrandDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetDetails(string name)
    {
        try
        {
            var result = await _mediator.Send(new GetBrandDetailsQuery(name));
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Create a new brand
    /// </summary>
    /// <param name="request"> Brand data transfer object </param>
    /// <returns> The task object containing the action result of creating a new brand </returns>
    /// <response code="201"> Successful completion </response>
    /// <response code="409"> Brand with this name already exists </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPost]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType(typeof(BrandDTOResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Create(BrandDTORequest request)
    {
        try
        {
            var result = await _mediator.Send(new CreateBrandCommand(request.Name, request.Description));

            return Created(
                new Uri($"http://localhost:44389/api/v1/CatalogAPI/Brand/GetDetails/{result.Id}"), result);
        }
        catch (ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Change brand data
    /// </summary>
    /// <param name="id"> Brand Id </param>
    /// <param name="request"> Brand data transfer object </param>
    /// <returns> The task object containing the action result of changing brand </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="409"> Brand with this name already exists </response>
    /// <response code="404"> Brand with this Id wasn't founded </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(Guid id, BrandDTORequest request)
    {
        try
        {
            await _mediator.Send(new UpdateBrandCommand(id, request.Name, request.Description));
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
}
