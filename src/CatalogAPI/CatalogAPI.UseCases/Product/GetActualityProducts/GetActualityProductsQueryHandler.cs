using AutoMapper;
using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.DTO;
using MediatR;

namespace CatalogAPI.UseCases.GetActualityProducts;

internal class GetActualityProductsQueryHandler :
    IRequestHandler<GetActualityProductsQuery, ProductListDTORabbitMQ<ProductDTORabbitMQ>>
{
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public GetActualityProductsQueryHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ProductListDTORabbitMQ<ProductDTORabbitMQ>> Handle(GetActualityProductsQuery request,
                                                                         CancellationToken cancellationToken)
    {
        ProductListDTORabbitMQ<ProductDTORabbitMQ> products = new();

        foreach (var productId in request.Model.Products)
        {
            var product = _db.Products.GetById(productId);

            if (product != null && product.IsSale)
                products.Products.Add(_mapper.Map<ProductDTORabbitMQ>(product));
            else
                products.Products.Add(null);
        }

        return products;
    }
}
