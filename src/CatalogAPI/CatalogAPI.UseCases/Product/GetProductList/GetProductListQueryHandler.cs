using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.UseCases.GetProductList;

public class GetProductListQueryHandler : IStreamRequestHandler<GetProductListQuery, ProductListDTOResponse>
{
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public GetProductListQueryHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async IAsyncEnumerable<ProductListDTOResponse> Handle(GetProductListQuery request,
                                                                 CancellationToken cancellationToken = default)
    {
        var products = _db.Products.GetAll().Where(x => x.IsSale);

        if (request.Specification.Filters != null)
        {
            var filters = request.Specification.Filters;

            if (filters.BrandId != null)
            {
                products = products.Where(x => x.BrandId == filters.BrandId);
            }

            if (filters.CategoryId != null)
            {
                products = products.Where(x => x.CategoryId == filters.CategoryId);
            }
        }

        if (!string.IsNullOrEmpty(request.Specification.SearchString))
        {
            products = products.Where(x => x.Name.ToLower().Contains(request.Specification.SearchString.ToLower()));
        }

        foreach (var product in products)
        {
            yield return _mapper.Map<ProductListDTOResponse>(product);
        }
    }
}
