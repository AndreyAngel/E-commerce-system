﻿using CatalogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DTO;
using CatalogAPI.Models.DataBase;
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
public class BrandController : ControllerBase
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Object of class <see cref="IBrandService"/> providing the APIs for managing category in a persistence store.
    /// </summary>
    private readonly IBrandService _service;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="BrandController"/>.
    /// </summary>
    /// <param name="unitOfWork"> Repository group interface showing data context </param>
    /// <param name="service"> Object of class <see cref="IBrandService"/>
    /// providing the APIs for managing category in a persistence store </param>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    public BrandController(IUnitOfWork unitOfWork, IBrandService service, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// Get the all brands information
    /// </summary>
    /// <returns> The action result of getting brands information </returns>
    /// <response code="200"> Successful completion </response>
    [HttpGet]
    [ProducesResponseType(typeof(List<BrandDTOResponse>), (int)HttpStatusCode.OK)]
    public IActionResult GetAll()
    {
        try
        {
            var result = _service.GetAll();
            var res = _mapper.Map<List<BrandDTOResponse>>(result);

            return Ok(res);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    /// <summary>
    /// Get the category information by Id
    /// </summary>
    /// <param name="id"> category Id </param>
    /// <returns> The action result of getting category information </returns>
    /// <response code="200"> Successful completion </response>
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(BrandDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var result = _service.GetById(id);
            var res = _mapper.Map<BrandDTOResponse>(result);

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

    /// <summary>
    /// Get the category information by name
    /// </summary>
    /// <param name="name"> category name </param>
    /// <returns> The action result of getting category information </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> category with this name wasn't founded </response>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(BrandDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public IActionResult GetByName(string name)
    {
        try
        {
            var result = _service.GetByName(name);
            var res = _mapper.Map<BrandDTOResponse>(result);

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
    /// Create a new category
    /// </summary>
    /// <param name="model"> category data transfer object </param>
    /// <returns> The task object containing the action result of creating a new category </returns>
    /// <response code="201"> Successful completion </response>
    /// <response code="409"> category with this name already exists </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPost]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType(typeof(BrandDTOResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Create(BrandDTORequest model)
    {
        try
        {
            Brand category = _mapper.Map<Brand>(model);

            var result = await _service.Create(category);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<BrandDTOResponse>(result);

            return Created(new Uri($"http://localhost:5192/api/v1/cat/category/GetById/{res.Id}"), res);
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

    /// <summary>
    /// Change category data
    /// </summary>
    /// <param name="id"> category Id </param>
    /// <param name="model"> category data transfer object </param>
    /// <returns> The task object containing the action result of changing category </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="409"> category with this name already exists </response>
    /// <response code="404"> category with this Id wasn't founded </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPut("{id:Guid}")]
    [Authorize(Policy = "ChangingOfCatalog")]
    [ProducesResponseType(typeof(BrandDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(Guid id, BrandDTORequest model)
    {
        try
        {
            Brand category = _mapper.Map<Brand>(model);
            category.Id = id;

            var result = await _service.Update(category);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<BrandDTOResponse>(result);

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
}
