using CatalogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Exceptions;
using CatalogAPI.Models.ViewModels;
using CatalogAPI.Models.DataBase;
using AutoMapper;
using CatalogAPI.UnitOfWork.Interfaces;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/brand/[action]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBrandService _service;
    private readonly IMapper _mapper;
    public BrandController(IUnitOfWork unitOfWork, IBrandService service, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<List<BrandViewModelResponce>> Get()
    {
        try
        {
            var result = _service.Get();
            var res = _mapper.Map<List<BrandViewModelResponce>>(result);

            return Ok(res);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpGet("{id:int}")]
    public ActionResult<BrandViewModelResponce> GetById(int id)
    {
        try
        {
            var result = _service.GetById(id);
            var res = _mapper.Map<BrandViewModelResponce>(result);

            return Ok(res);
        }
        catch(ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpGet("{name}")]
    public ActionResult<BrandViewModelResponce> GetByName(string name)
    {
        try
        {
            var result = _service.GetByName(name);
            var res = _mapper.Map<BrandViewModelResponce>(result);

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
    public async Task<ActionResult<BrandViewModelResponce>> Create(BrandViewModelRequest model)
    {
        try
        {
            Brand brand = _mapper.Map<Brand>(model);

            var result = await _service.Create(brand);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<BrandViewModelResponce>(result);

            return Created(new Uri($"http://localhost:5192/api/v1/cat/brand/GetById/{res.Id}"), res);
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

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BrandViewModelResponce>> Update(int id, BrandViewModelRequest model)
    {
        try
        {
            Brand brand = _mapper.Map<Brand>(model);
            brand.Id = id;

            var result = await _service.Update(brand);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<BrandViewModelResponce>(result);

            return Ok(res);
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
