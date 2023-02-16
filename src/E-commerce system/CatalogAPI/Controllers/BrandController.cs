using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<Brand>>> Get()
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
    public async Task<ActionResult<Brand>> GetById(int id)
    {
        try
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<ActionResult<Brand>> GetByName(string name)
    {
        try
        {
            var result = await _service.GetByName(name);
            return Ok(result);
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
            return Ok(result);
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
