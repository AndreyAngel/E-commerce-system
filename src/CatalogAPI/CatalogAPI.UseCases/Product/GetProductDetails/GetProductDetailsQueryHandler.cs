using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.GetProductDetails;

public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, ProductDTOResponse>
{
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public GetProductDetailsQueryHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ProductDTOResponse> Handle(GetProductDetailsQuery request, CancellationToken token)
    {
        Product? product;

        // Search by ID
        if (request.Id !=  Guid.Empty)
        {
            product = _db.Products.Include(x => x.Category, x => x.Brand).SingleOrDefault(x => x.Id == request.Id);

            if (product == null)
            {
                throw new NotFoundException("Product with this Id was not founded!", nameof(request.Id));
            }
        }
        // Search by name
        else
        {
            product = _db.Products.Include(x => x.Category, x => x.Brand)
                                  .SingleOrDefault(x => x.Name == request.Name);

            if (product == null)
            {
                throw new NotFoundException("Product with this name was not founded!", nameof(request.Name));
            }
        }

        return _mapper.Map<ProductDTOResponse>(product);
    }
}
