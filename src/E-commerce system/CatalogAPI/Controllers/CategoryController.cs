using AutoMapper;
using CatalogAPI.Models.ViewModels;
using CatalogAPI.Models.DataBase;
using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using CatalogAPI.UnitOfWork.Interfaces;
using CatalogAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/category/[action]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryService _service;
    private readonly IMapper _mapper;
    public CategoryController(IUnitOfWork unitOfWork, ICategoryService service, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpGet("{id:int}")]
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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpGet("{name}")]
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
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<CategoryViewModelResponce>> Create(CategoryViewModelRequest model)
    {
        try
        {
            Category category = _mapper.Map<Category>(model);

            var result = await _service.Create(category);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<CategoryViewModelResponce>(result);

            return Created(new Uri($"https://localhost:5192/api/v1/cat/category/GetById/{result.Id}"), res);
        }
        catch(ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<CategoryViewModelResponce>> Update(int id, CategoryViewModelRequest model)
    {
        try
        {
            Category category = _mapper.Map<Category>(model);
            category.Id = id;

            var result = await _service.Update(category);
            await _unitOfWork.SaveChangesAsync();

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
        catch (ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }
}
