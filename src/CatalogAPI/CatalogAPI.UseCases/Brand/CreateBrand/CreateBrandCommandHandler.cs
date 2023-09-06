using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.CreateBrand;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, BrandDTOResponse>
{
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public CreateBrandCommandHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<BrandDTOResponse> Handle(CreateBrandCommand request, CancellationToken token = default)
    {
        if (_db.Brands.GetAll().SingleOrDefault(x => x.Name == request.Name) != null)
        {
            throw new ObjectNotUniqueException("Brand with this name alredy exists!", nameof(request.Name));
        }

        var brand = new Brand(request.Name, request.Description);
        await _db.Brands.AddAsync(brand);
        await _db.SaveChangesAsync();

        return _mapper.Map<BrandDTOResponse>(brand);
    }
}
