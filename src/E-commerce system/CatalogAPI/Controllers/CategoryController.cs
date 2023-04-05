using AutoMapper;
using OrderAPI.Models.ViewModels;
using OrderAPI.Models.DataBase;
using OrderAPI.Services.Interfaces;
using OrderAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.UnitOfWork.Interfaces;
using OrderAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace OrderAPI.Controllers;


[Route("api/v1/cat/[controller]/[action]")]
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
    [Authorize(Policy = "Public")]
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

    [HttpGet("{id:Guid}")]
    [Authorize(Policy = "Public")]
    public ActionResult<CategoryViewModelResponce> GetById(Guid id)
    {
        try
        {
            var result = _service.GetById(id);
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

    [HttpGet("{name}")]
    [Authorize(Policy = "Public")]
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
    [Authorize(Policy = "ChangingOfCatalog")]
    public async Task<ActionResult<CategoryViewModelResponce>> Create(CategoryViewModelRequest model)
    {
        try
        {
            Category category = _mapper.Map<Category>(model);

            var result = await _service.Create(category);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<CategoryViewModelResponce>(result);

            return Created(new Uri($"https://localhost:5192/api/v1/cat/Category/GetById/{result.Id}"), res);
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

    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    public async Task<ActionResult<CategoryViewModelResponce>> Update(Guid id, CategoryViewModelRequest model)
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
