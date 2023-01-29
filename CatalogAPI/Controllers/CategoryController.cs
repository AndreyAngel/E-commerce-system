using CatalogAPI.Models;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/category/[action]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _service;
    public CategoryController(CategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> Get()
    {
        try
        {
            var result = await _service.Get();
            return Ok(result);
        }
        catch(Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<Category>> GetById(int id)
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
    public async Task<ActionResult<Category>> GetByName(string name)
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
    public async Task<ActionResult<Category>> Create(Category category)
    {
        try
        {
            var result = await _service.Create(category);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<Category>> Update(Category category)
    {
        try
        {
            var result = await _service.Update(category);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
