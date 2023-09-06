using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Repositories.Interfaces;
using CatalogAPI.UseCases.GetCategoriesList;
using MediatR;

namespace CatalogAPI.UseCases.GetCategoryList;

public class GetCategoryListQueryHandler : IStreamRequestHandler<GetCategoryListQuery, CategoryDTOResponse>
{
    private readonly IUnitOfWork _db;

    private IMapper _mapper;

    public GetCategoryListQueryHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async IAsyncEnumerable<CategoryDTOResponse> Handle(GetCategoryListQuery request,
                                                              CancellationToken cancellationToken = default)
    {
        // Реализовать поиск и сортировку
        foreach (var category in _db.Categories.GetAll())
        {
            yield return _mapper.Map<CategoryDTOResponse>(category);
        }
    }
}
