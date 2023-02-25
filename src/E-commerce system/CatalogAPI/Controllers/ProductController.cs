using Microsoft.AspNetCore.Mvc;
using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/product/[action]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get()
    {
        try
        {
            var result = await _service.Get();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        try
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<ActionResult<Product>> GetByName(string name)
    {
        try
        {
            var result = await _service.GetByName(name);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{brandId:int}")]
    public async Task<ActionResult<Product>> GetByBrandId(int brandId)
    {
        try
        {
            var result = await _service.GetByBrandId(brandId);
            return Ok(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{brandName}")]
    public async Task<ActionResult<Product>> GetByBrandName(string brandName)
    {
        try
        {
            var result = await _service.GetByBrandName(brandName);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{categoryId:int}")]
    public async Task<ActionResult<Product>> GetByCategoryId(int categoryId)
    {
        try
        {
            var result = await _service.GetByCategoryId(categoryId);
            return Ok(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{categoryName}")]
    public async Task<ActionResult<Product>> GetByCategoryName(string categoryName)
    {
        try
        {
            var result = await _service.GetByCategoryName(categoryName);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        try
        {
            var result = await _service.Create(product);
            return Created(new Uri($"http://localhost:5192/api/v1/cat/product/GetById/{result.Id}"), result);
        }
        catch (ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<Product>> Update(Product product)
    {
        try
        {
            var result = await _service.Update(product);
            return Ok(result);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}