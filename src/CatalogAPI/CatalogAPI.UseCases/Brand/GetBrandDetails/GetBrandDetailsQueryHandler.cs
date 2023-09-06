using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.GetBrandDetails;

public class GetBrandDetailsQueryHandler : IRequestHandler<GetBrandDetailsQuery, BrandDTOResponse>
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public GetBrandDetailsQueryHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<BrandDTOResponse> Handle(GetBrandDetailsQuery request, CancellationToken token = default)
    {
        Brand? brand;

        // Search by ID
        if (request.Id != Guid.Empty)
        {
            brand = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Id == request.Id);

            if (brand == null)
            {
                throw new NotFoundException("Brand with this Id was not founded!", nameof(request.Id));
            }
        }
        // Search by name
        else
        {
            brand = _db.Brands.Include(x => x.Products).SingleOrDefault(x => x.Name == request.Name);

            if (brand == null)
            {
                throw new NotFoundException("Brand with this name was not founded!", nameof(request.Name));
            }
        }

        return _mapper.Map<BrandDTOResponse>(brand);
    }
}