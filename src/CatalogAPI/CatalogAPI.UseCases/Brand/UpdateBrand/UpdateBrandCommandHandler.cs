using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.UpdateBrand;

public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand>
{
    private IUnitOfWork _db;

    public UpdateBrandCommandHandler(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task Handle(UpdateBrandCommand request, CancellationToken token = default)
    {
        var brand = _db.Brands.GetById(request.Id);

        if (brand == null)
        {
            throw new NotFoundException("Brand with this Id was not founded!", nameof(request.Id));
        }

        else if ((brand.Name != request.Name) && _db.Brands.GetAll().SingleOrDefault(x => x.Name == request.Name) != null)
        {
            throw new ObjectNotUniqueException("Brand with this name already exists!", nameof(request.Name));
        }

        brand.Name = request.Name ?? brand.Name;
        brand.Description = request.Description ?? brand.Description;

        await _db.Brands.UpdateAsync(brand);
        await _db.SaveChangesAsync();
    }
}
