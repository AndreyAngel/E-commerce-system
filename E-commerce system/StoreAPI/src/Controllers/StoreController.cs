using AutoMapper;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DataBase.Entities;
using StoreAPI.Models.DTO;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers;

[Route("api/v1/StoreAPI/[controller]/[action]")]
[ApiController]
public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    private readonly IStockService _stockService;

    private readonly IMapper _mapper;

    public StoreController(IStoreService storeService, IStockService stockService, IMapper mapper)
    {
        _storeService = storeService;
        _stockService = stockService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var stores = _storeService.GetAll();
        var response = _mapper.Map<List<StoreDTOResponse>>(stores);

        return Ok(response);
    }

    [HttpGet("{Id:Guid}")]
    public IActionResult GetById(Guid Id)
    {
        try
        {
            var store = _storeService.GetById(Id);
            var response = _mapper.Map<StoreDTOResponse>(store);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{storeId:Guid}")]
    public IActionResult GetStockByStoreId(Guid storeId)
    {
        try
        {
            var stock = _stockService.GetByStoreId(storeId);
            var response = _mapper.Map<StockProductDTOResponse>(stock);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{storeId:Guid}")]
    public IActionResult GetStoreProductsByStoreId(Guid storeId)
    {
        try
        {
            var storeProducts = _storeService.GetStoreProductsByStoreId(storeId);
            var response = _mapper.Map<List<StoreProductDTOResponse>>(storeProducts);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{storeId:Guid},{productId:Guid}")]
    public IActionResult GetProductQuanityByProductId(Guid storeId, Guid productId)
    {
        try
        {
            var stockProduct = _stockService.GetStockProductByProductId(storeId, productId);
            var storeProduct = _storeService.GetStoreProductByProductId(storeId, productId);
            var response = stockProduct.Quantity + storeProduct.Quantity;

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Store store)
    {
        try
        {
            var result = await _storeService.Create(store);
            var response = _mapper.Map<StoreDTOResponse>(store);

            return Ok(response);
        }
        catch (ObjectNotUniqueException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToStock(StockProductDTORequest model)
    {
        var stockProduct = _mapper.Map<StockProduct>(model);
        var result = await _stockService.AddProduct(stockProduct);
        var response = _mapper.Map<StockProductDTOResponse>(result);

        return Ok(response);
    }

    [HttpPatch("{storeId:Guid},{stockProductId:Guid},{quantity:int}")]
    public async Task<IActionResult> TakeProductFromStock(Guid storeId, Guid stockProductId, int quantity)
    {
        try
        {
            var stockProduct = await _stockService.TakeProductFromStock(storeId, stockProductId, quantity);
            var storeProduct = _mapper.Map<StoreProduct>(stockProduct);
            await _storeService.TakeProductFromStock(storeProduct);

            return Ok(storeProduct);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{storeId:Guid},{storeProductId:Guid},{quantity:int}")]
    public async Task<IActionResult> ReturnProductToStock(Guid storeId, Guid storeProductId, int quantity)
    {
        try
        {
            var storeProduct = await _storeService.ReturnProductToStock(storeId, storeProductId, quantity);
            var stockProduct = _mapper.Map<StockProduct>(storeProduct);
            await _stockService.ReturnProductToStock(stockProduct);

            return Ok(storeProduct);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{storeId:Guid},{productId:Guid}")]
    public async Task<IActionResult> SellProduct(Guid storeId, Guid productId)
    {
        try
        {
            await _storeService.SellProduct(storeId, productId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{storeId:Guid},{stockProductId:Guid},{quantity:int}")]
    public async Task<IActionResult> ReturnProductToStore(Guid storeId, Guid stockProductId, int quantity)
    {
        try
        {
            var result = await _storeService.ReturnProductToStore(storeId, stockProductId, quantity);
            var response = _mapper.Map<StoreProductDTOResponse>(result);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{storeId:Guid},{stockProductId:Guid},{quantity:int}")]
    public async Task<IActionResult> WriteOffTheProduct(Guid storeId, Guid stockProductId, int quantity)
    {
        try
        {
            await _stockService.WriteOffTheProduct(storeId, stockProductId, quantity);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{storeId:Guid}")]
    public async Task<IActionResult> Update(Guid storeId, StoreDTORequest model)
    {
        try
        {
            var store = _mapper.Map<Store>(model);
            store.Id = storeId;
            var result = await _storeService.Update(store);
            var response = _mapper.Map<StoreDTOResponse>(store);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ObjectNotUniqueException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{storeId:Guid}")]
    public async Task<IActionResult> Delete(Guid storeId)
    {
        try
        {
            await _storeService.Delete(storeId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
