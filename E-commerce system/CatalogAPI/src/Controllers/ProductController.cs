using Microsoft.AspNetCore.Mvc;
using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DataBase;
using CatalogAPI.Models.DTO;
using AutoMapper;
using CatalogAPI.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CatalogAPI.Controllers;


[Route("api/v1/CatalogAPI/[controller]/[action]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _service;
    private readonly IMapper _mapper;
    public ProductController(IUnitOfWork unitOfWork, IProductService service, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Policy = "Public")]
    public ActionResult<List<ProductListDTOResponce>> GetAll()
    {
        try
        {
            var result = _service.Get();
            var res = _mapper.Map<List<ProductListDTOResponce>>(result);

            return Ok(res);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpGet("{id:Guid}")]
    [Authorize(Policy = "Public")]
    public ActionResult<ProductDTOResponce> GetById(Guid id)
    {
        try
        {
            var result = _service.GetById(id);
            var res = _mapper.Map<ProductDTOResponce>(result);

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
    public ActionResult<ProductDTOResponce> GetByName(string name)
    {
        try
        {
            var result = _service.GetByName(name);
            var res = _mapper.Map<ProductDTOResponce>(result);

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
    [Authorize(Policy = "Public")]
    public ActionResult< List<ProductListDTOResponce> > GetByFilter(ProductFilterDTO filter)
    {
        try
        {
            var result = _mapper.Map<List<ProductListDTOResponce>>(_service.GetByFilter(filter));
            return Ok(result);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPost]
    [Authorize(Policy = "ChangingOfCatalog")]
    public async Task<ActionResult<ProductDTOResponce>> Create(ProductDTORequest model)
    {
        try
        {
            Product product = _mapper.Map<Product>(model);

            var result = await _service.Create(product);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductDTOResponce>(result);

            return Created(new Uri($"http://localhost:5192/api/v1/cat/Product/GetById/{res.Id}"), res);
        }
        catch (ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
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

    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    public async Task<ActionResult<ProductDTOResponce>> Update(Guid id, ProductDTORequest model)
    {
        try
        {
            Product product = _mapper.Map<Product>(model);
            product.Id = id;

            var result = await _service.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductDTOResponce>(result);

            return Ok(res);
        }
        catch (ObjectNotUniqueException ex)
        {
            return Conflict(ex.Message);
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

    [HttpDelete("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var res = _service.GetById(id);
            res.IsSale = false;
            await _service.Update(res);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
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
}