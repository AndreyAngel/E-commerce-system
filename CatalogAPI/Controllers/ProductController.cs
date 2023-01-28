using Microsoft.AspNetCore.Mvc;
using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService service;
    public ProductController(IProductService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get()
    {
        try
        {
            var result = await service.Get();
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
            var result = await service.GetById(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{name:string}")]
    public async Task<ActionResult<Product>> GetByName(string name)
    {
        try
        {
            var result = await service.GetByName(name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
