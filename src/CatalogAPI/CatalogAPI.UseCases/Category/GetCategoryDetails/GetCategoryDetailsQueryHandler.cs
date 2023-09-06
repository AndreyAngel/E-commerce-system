using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.GetCategoryDetails;

public class GetCategoryDetailsQueryHandler : IRequestHandler<GetCategoryDetailsQuery, CategoryDTOResponse>
{
    /// <summary>
    /// Repository group interface showing data context
    /// </summary>
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public GetCategoryDetailsQueryHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CategoryDTOResponse> Handle(GetCategoryDetailsQuery request, CancellationToken token = default)
    {
        Category? category;

        // Search by ID
        if (request.Id != Guid.Empty)
        {
            category = _db.Categories.Include(x => x.Products).SingleOrDefault(x => x.Id == request.Id);

            if (category == null)
            {
                throw new NotFoundException("Category with this Id was not founded!", nameof(request.Id));
            }
        }
        // Search by name
        else
        {
            category = _db.Categories.Include(x => x.Products).SingleOrDefault(x => x.Name == request.Name);

            if (category == null)
            {
                throw new NotFoundException("Category with this name was not founded!", nameof(request.Name));
            }
        }

        return _mapper.Map<CategoryDTOResponse>(category);
    }
}