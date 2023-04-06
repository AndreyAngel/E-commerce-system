using CatalogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DTO;
using CatalogAPI.Models.DataBase;
using AutoMapper;
using CatalogAPI.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/[controller]/[action]")]
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
    [Authorize(Policy = "Public")]
    public ActionResult<List<BrandDTOResponce>> Get()
    {
        try
        {
            var result = _service.Get();
            var res = _mapper.Map<List<BrandDTOResponce>>(result);

            return Ok(res);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpGet("{id:Guid}")]
    [Authorize(Policy = "Public")]
    public ActionResult<BrandDTOResponce> GetById(Guid id)
    {
        try
        {
            var result = _service.GetById(id);
            var res = _mapper.Map<BrandDTOResponce>(result);

            return Ok(res);
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
    [Authorize(Policy = "Public")]
    public ActionResult<BrandDTOResponce> GetByName(string name)
    {
        try
        {
            var result = _service.GetByName(name);
            var res = _mapper.Map<BrandDTOResponce>(result);

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
    public async Task<ActionResult<BrandDTOResponce>> Create(BrandDTORequest model)
    {
        try
        {
            Brand brand = _mapper.Map<Brand>(model);

            var result = await _service.Create(brand);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<BrandDTOResponce>(result);

            return Created(new Uri($"http://localhost:5192/api/v1/cat/Brand/GetById/{res.Id}"), res);
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

    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    public async Task<ActionResult<BrandDTOResponce>> Update(Guid id, BrandDTORequest model)
    {
        try
        {
            Brand brand = _mapper.Map<Brand>(model);
            brand.Id = id;

            var result = await _service.Update(brand);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<BrandDTOResponce>(result);

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
