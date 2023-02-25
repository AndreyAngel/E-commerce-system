﻿using CatalogAPI.Models;
using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/category/[action]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    public CategoryController(ICategoryService service)
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
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
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
    public async Task<ActionResult<Category>> GetByName(string name)
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

    [HttpPost]
    public async Task<ActionResult<Category>> Create(Category category)
    {
        try
        {
            var result = await _service.Create(category);
            return Created(new Uri($"http://localhost:5192/api/v1/cat/category/GetById/{result.Id}"), result);
        }
        catch(ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
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
