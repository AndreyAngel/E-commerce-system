using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDTOResponse>
{
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CategoryDTOResponse> Handle(CreateCategoryCommand request, CancellationToken token = default)
    {
        if (_db.Categories.GetAll().SingleOrDefault(x => x.Name == request.Name) != null)
        {
            throw new ObjectNotUniqueException("Category with this name alredy exists!", nameof(request.Name));
        }

        var category = new Category(request.Name, request.Description);
        await _db.Categories.AddAsync(category);
        await _db.SaveChangesAsync();

        return _mapper.Map<CategoryDTOResponse>(category);
    }
}
