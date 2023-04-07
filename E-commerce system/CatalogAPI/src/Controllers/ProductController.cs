using Microsoft.AspNetCore.Mvc;
using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DataBase;
using CatalogAPI.Models.DTO;
using AutoMapper;
using CatalogAPI.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace CatalogAPI.Controllers;

/// <summary>
/// Provides the APIs for handling all the category logic
/// </summary>
[Route("api/v1/CatalogAPI/[controller]/[action]")]
[ApiController]
public class ProductController : ControllerBase
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Object of class <see cref="IProductService"/> providing the APIs for managing product in a persistence store.
    /// </summary>
    private readonly IProductService _service;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="ProductController"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    /// <param name="service"> Object of class <see cref="IProductService"/>
    /// providing the APIs for managing product in a persistence store </param>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    public ProductController(IUnitOfWork unitOfWork, IProductService service, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Get the all products information
    /// </summary>
    /// <returns> The action result of getting products information </returns>
    /// <response code="200"> Successful completion </response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductListDTOResponse>), (int)HttpStatusCode.OK)]
    public IActionResult GetAll()
    {
        try
        {
            var result = _service.GetAll();
            var res = _mapper.Map<List<ProductListDTOResponse>>(result);

            return Ok(res);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    /// <summary>
    /// Get the product information by Id
    /// </summary>
    /// <param name="id"> Product Id </param>
    /// <returns> The action result of getting product information </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Product with this Id wasn't founded </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var result = _service.GetById(id);
            var res = _mapper.Map<ProductDTOResponse>(result);

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

    /// <summary>
    /// Get the product information by name
    /// </summary>
    /// <param name="name"> Product name </param>
    /// <returns> The action result of getting product information </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Product with this name wasn't founded </response>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public IActionResult GetByName(string name)
    {
        try
        {
            var result = _service.GetByName(name);
            var res = _mapper.Map<ProductDTOResponse>(result);

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

    /// <summary>
    /// Get the products information by filters
    /// </summary>
    /// <param name="filter"> Filters </param>
    /// <returns> The action resuslt to getting information about filtered products </returns>
    /// <response code="200"> Successful completion </response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.OK)]
    public IActionResult GetByFilter(ProductFilterDTO filter)
    {
        try
        {
            var result = _mapper.Map<List<ProductListDTOResponse>>(_service.GetByFilter(filter));
            return Ok(result);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    /// <summary>
    /// Create a new product or put it back on sale
    /// </summary>
    /// <param name="model"> Product data transfer object </param>
    /// <returns> The task object containing the action result of creating a new product </returns>
    /// <response code="201"> Successful completion </response>
    /// <response code="409"> Product with this name already exists </response>
    /// <response code="404"> category or category with this id wasn't founded </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPost]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Create(ProductDTORequest model)
    {
        try
        {
            Product product = _mapper.Map<Product>(model);

            var result = await _service.Create(product);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductDTOResponse>(result);

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

    /// <summary>
    /// Change product data
    /// </summary>
    /// <param name="id"> Product Id </param>
    /// <param name="model"> Product data transfer object </param>
    /// <returns> The task object containing the action result of changing product </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="409"> Product with this name already exists </response>
    /// <response code="404"> Product, category or category with this Id wasn't founded </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(Guid id, ProductDTORequest model)
    {
        try
        {
            Product product = _mapper.Map<Product>(model);
            product.Id = id;

            var result = await _service.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductDTOResponse>(result);

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

    /// <summary>
    /// Remove a product from the public domain.
    /// Product is withdrawn from sale.
    /// </summary>
    /// <param name="id"> Product Id </param>
    /// <returns> The task object containing the action result of delete product </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Product with this Id wasn't founded </response>
    [HttpDelete("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType(typeof(ProductDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(Guid id)
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