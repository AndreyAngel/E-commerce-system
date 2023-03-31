﻿using Microsoft.AspNetCore.Mvc;
using CatalogAPI.Services.Interfaces;
using Infrastructure.Exceptions;
using CatalogAPI.Models.DataBase;
using CatalogAPI.Models.ViewModels;
using AutoMapper;
using CatalogAPI.UnitOfWork.Interfaces;
using CatalogAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CatalogAPI.Controllers;


[Route("api/v1/cat/product/[action]")]
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

    [HttpGet("{id:int}")]
    public ActionResult<ProductViewModelResponce> GetById(int id)
    {
        try
        {
            var result = _service.GetById(id);
            var res = _mapper.Map<ProductViewModelResponce>(result);

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
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<ProductViewModelResponce>> Create(ProductViewModelRequest model)
    {
        try
        {
            Product product = _mapper.Map<Product>(model);

            var result = await _service.Create(product);
            await _unitOfWork.SaveChangesAsync();

            var res = _mapper.Map<ProductViewModelResponce>(result);

            return Created(new Uri($"http://localhost:5192/api/v1/cat/product/GetById/{res.Id}"), res);
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

    [HttpPut("{id:int}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<ProductViewModelResponce>> Update(int id, ProductViewModelRequest model)
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
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
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

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var res = _service.GetById(id);
            res.IsSale = false;
            await _service.Update(res);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
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
}