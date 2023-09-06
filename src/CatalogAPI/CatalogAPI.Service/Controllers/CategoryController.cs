using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using CatalogAPI.Contracts.DTO;
using MediatR;
using CatalogAPI.UseCases.GetCategoriesList;
using CatalogAPI.UseCases.GetCategoryDetails;
using CatalogAPI.UseCases.CreateCategory;
using CatalogAPI.UseCases.UpdateCategory;

namespace CatalogAPI.Controllers;


/// <summary>
/// Provides the APIs for handling all the category logic
/// </summary>
[Route("api/v1/CatalogAPI/[controller]/[action]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Creates an instance of the <see cref="CategoryController"/>.
    /// </summary>
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get the all categories information
    /// </summary>
    /// <returns> The action result of getting categories information </returns>
    /// <response code="200"> Successful completion </response>
    [HttpGet()]
    [ProducesResponseType(typeof(IAsyncEnumerable<CategoryDTOResponse>), (int)HttpStatusCode.OK)]
    public ActionResult<IAsyncEnumerable<CategoryDTOResponse>> GetList(string? sort = null,
                                                                       string? searchString = null)
    {
        var result = _mediator.CreateStream(new GetCategoryListQuery(sort, searchString));
        return Ok(result);
    }

    /// <summary>
    /// Get the category information by Id
    /// </summary>
    /// <param name="id"> Category Id </param>
    /// <returns> The action result of getting category information </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Category with this Id wasn't founded </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(CategoryDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetCategoryDetailsQuery(id));
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get the category information by name
    /// </summary>
    /// <param name="name"> Category name </param>
    /// <returns> The action result of getting category information </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Category with this name wasn't founded </response>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(CategoryDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetDetails(string name)
    {
        try
        {
            var result = await _mediator.Send(new GetCategoryDetailsQuery(name));
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="request"> Category data transfer object </param>
    /// <returns> The task object containing the action result of creating a new category </returns>
    /// <response code="201"> Successful completion </response>
    /// <response code="409"> Category with this name already exists </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPost]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType(typeof(CategoryDTOResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Create(CategoryDTORequest request)
    {
        try
        {
            var result = await _mediator.Send(new CreateCategoryCommand(request.Name, request.Description));

            return Created(
                new Uri($"https://localhost:44389/api/v1/CatalogAPI/Category/GetDetails/{result.Id}"), result);
        }
        catch(ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// Change category data
    /// </summary>
    /// <param name="id"> Category Id </param>
    /// <param name="request"> Category data transfer object </param>
    /// <returns> The task object containing the action result of changing category </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="409"> Category with this name already exists </response>
    /// <response code="404"> Category with this Id wasn't founded </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(Guid id, CategoryDTORequest request)
    {
        try
        {
            await _mediator.Send(new UpdateCategoryCommand(id, request.Name, request.Description));
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
