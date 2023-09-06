using AutoMapper;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _db;

    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request.Model);
        product.Id = request.Id;

        var res = _db.Products.GetById(product.Id);

        if ((res.Name != product.Name) && _db.Products.GetAll().SingleOrDefault(x => x.Name == product.Name) != null)
        {
            throw new ObjectNotUniqueException("Product with this name already exists!", nameof(product.Name));
        }

        else if (res == null || !res.IsSale)
        {
            throw new NotFoundException("Product with this Id was not founded!", nameof(product.Id));
        }

        else if (_db.Brands.GetById(product.BrandId) == null)
        {
            throw new NotFoundException("Brand with this Id was not founded!", nameof(product.BrandId));
        }

        else if (_db.Categories.GetById(product.CategoryId) == null)
        {
            throw new NotFoundException("Category with this Id was not founded!", nameof(product.CategoryId));
        }

        await _db.Products.UpdateAsync(product);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
