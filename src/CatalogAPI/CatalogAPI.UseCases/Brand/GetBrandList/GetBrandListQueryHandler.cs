using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Repositories.Interfaces;
using CatalogAPI.UseCases.GetBrandsList;
using MediatR;

namespace CatalogAPI.UseCases.GetBrandList;

public class GetBrandListQueryHandler : IStreamRequestHandler<GetBrandListQuery, BrandDTOResponse>
{
    private readonly IUnitOfWork _db;

    private IMapper _mapper;

    public GetBrandListQueryHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async IAsyncEnumerable<BrandDTOResponse> Handle(GetBrandListQuery request,
                                                           CancellationToken cancellationToken = default)
    {
        // TODO: реализовать поиск и сортировку
        foreach (var brand in _db.Brands.GetAll())
        {
            yield return _mapper.Map<BrandDTOResponse>(brand);
        }
    }
}
