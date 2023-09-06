using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDTOResponse>
{
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ProductDTOResponse> Handle(CreateProductCommand request, CancellationToken token)
    {
        var product = _mapper.Map<Product>(request.Model);

        var res = _db.Products.GetAll().SingleOrDefault(x => x.Name == product.Name);

        if (res != null && res.IsSale)
        {
            throw new ObjectNotUniqueException("Product with this name already exists!", nameof(product.Name));
        }

        else if (_db.Brands.GetById(product.BrandId) == null)
        {
            throw new NotFoundException("Brand with this Id was not founded!", nameof(product.BrandId));
        }

        else if (_db.Categories.GetById(product.CategoryId) == null)
        {
            throw new NotFoundException("Category with this Id was not founded!", nameof(product.CategoryId));
        }

        else if (res != null && !res.IsSale)
        {
            res.IsSale = true;
            await _db.Products.UpdateAsync(res);
            await _db.SaveChangesAsync();

            return _mapper.Map<ProductDTOResponse>(_db.Products.GetById(product.Id));
        }

        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();

        product.Category = _db.Categories.GetById(product.CategoryId);
        product.Brand = _db.Brands.GetById(product.BrandId);

        return _mapper.Map<ProductDTOResponse>(product);
    }
}
