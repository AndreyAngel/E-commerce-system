using AutoMapper;
using CatalogAPI.Models.ViewModels;
using CatalogAPI.Models.DataBase;
using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/category/[action]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    private readonly IMapper _mapper;
    public CategoryController(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<List<CategoryViewModelResponce>> Get()
    {
        try
        {
            var result = _service.Get();
            var res = _mapper.Map<List<CategoryViewModelResponce>>(result);

            return Ok(res);
        }
        catch(Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public ActionResult<CategoryViewModelResponce> GetById(int id)
    {
        try
        {
            var result = _service.GetById(id);
            var res = _mapper.Map<CategoryViewModelResponce>(result);

            return Ok(res);
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
    public ActionResult<CategoryViewModelResponce> GetByName(string name)
    {
        try
        {
            var result = _service.GetByName(name);
            var res = _mapper.Map<CategoryViewModelResponce>(result);

            return Ok(res);
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
    public async Task<ActionResult<CategoryViewModelResponce>> Create(CategoryViewModelRequest model)
    {
        try
        {
            Category category = _mapper.Map<Category>(model);
            var result = await _service.Create(category);
            var res = _mapper.Map<CategoryViewModelResponce>(result);

            return Created(new Uri($"http://localhost:5192/api/v1/cat/category/GetById/{result.Id}"), res);
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
    [Route("{id:int}")]
    public async Task<ActionResult<CategoryViewModelResponce>> Update(int id, CategoryViewModelRequest model)
    {
        try
        {
            Category category = _mapper.Map<Category>(model);
            category.Id = id;

            var result = await _service.Update(category);
            var res = _mapper.Map<CategoryViewModelResponce>(result);

            return Ok(res);
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
