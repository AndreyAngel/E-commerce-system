using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Exceptions;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/brand/[action]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IBrandService _service;
    public BrandController(IBrandService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<List<Brand>> Get()
    {
        try
        {
            var result = _service.Get();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public ActionResult<Brand> GetById(int id)
    {
        try
        {
            var result = _service.GetById(id);
            return Ok(result);
        }
        catch(ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
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
    public ActionResult<Brand> GetByName(string name)
    {
        try
        {
            var result = _service.GetByName(name);
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
    public async Task<ActionResult<Brand>> Create(Brand brand)
    {
        try
        {
            var result = await _service.Create(brand);
            return Created(new Uri($"http://localhost:5192/api/v1/cat/brand/GetById/{result.Id}"), result);
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
    public async Task<ActionResult<Brand>> Update(Brand brand)
    {
        try
        {
            var result = await _service.Update(brand);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
