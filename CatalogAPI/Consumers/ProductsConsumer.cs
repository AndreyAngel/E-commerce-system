﻿using MassTransit;
using Infrastructure.DTO;
using CatalogAPI.Services.Interfaces;

namespace CatalogAPI.Consumers;

public class ProductsConsumer : IConsumer<ProductList<int>>
{
    private readonly IProductService _service;

    public ProductsConsumer(IProductService service)
    {
        _service = service;
    }
    public async Task Consume(ConsumeContext<ProductList<int>> context)
    {
        var content = context.Message;
        var res = await _service.CheckProducts(content);
        await context.RespondAsync(res);
    }
}
