using Microsoft.AspNetCore.Mvc;
using OrderAPI.Services.Interfaces;
using OrderAPI.Exceptions;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.ViewModels;
using AutoMapper;
using OrderAPI.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace OrderAPI.Controllers;


[Route("api/v1/cat/[controller]/[action]")]
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
    public ActionResult<List<ProductListViewModelResponce>> Get()
    {
        try
        {
            var result = _service.Get();
            var res = _mapper.Map<List<ProductListViewModelResponce>>(result);

            return Ok(res);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpGet("{id:Guid}")]
    [Authorize(Policy = "Public")]
    public ActionResult<ProductViewModelResponce> GetById(Guid id)
    {
        try
        {
            var result = _service.GetById(id);
            var res = _mapper.Map<ProductViewModelResponce>(result);

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
    public ActionResult<ProductViewModelResponce> GetByName(string name)
    {
        try
        {
            var result = _service.GetByName(name);
            var res = _mapper.Map<ProductViewModelResponce>(result);

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
    public ActionResult< List<ProductListViewModelResponce> > GetByFilter(ProductFilterViewModel filter)
    {
        try
        {
            var result = _mapper.Map<List<ProductListViewModelResponce>>(_service.GetByFilter(filter));
            return Ok(result);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    [HttpPost]
    [Authorize(Policy = "ChangingOfCatalog")]
    public async Task<ActionResult<ProductViewModelResponce>> Create(ProductViewModelRequest model)
    {
        try
        {
            Product product = _mapper.Map<Product>(model);

            var result = await _service.Create(product);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductViewModelResponce>(result);

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
    public async Task<ActionResult<ProductViewModelResponce>> Update(Guid id, ProductViewModelRequest model)
    {
        try
        {
            Product product = _mapper.Map<Product>(model);
            product.Id = id;

            var result = await _service.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductViewModelResponce>(result);

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