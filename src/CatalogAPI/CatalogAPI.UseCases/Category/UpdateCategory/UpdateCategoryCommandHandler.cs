using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private IUnitOfWork _db;

    public UpdateCategoryCommandHandler(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken token = default)
    {
        var category = _db.Categories.GetById(request.Id);

        if (category == null)
        {
            throw new NotFoundException("Brand with this Id was not founded!", nameof(request.Id));
        }

        else if ((category.Name != request.Name) && _db.Brands.GetAll()
                                                    .SingleOrDefault(x => x.Name == request.Name) != null)
        {
            throw new ObjectNotUniqueException("Brand with this name already exists!", nameof(request.Name));
        }

        category.Name = request.Name ?? category.Name;
        category.Description = request.Description ?? category.Description;

        await _db.Categories.UpdateAsync(category);
        await _db.SaveChangesAsync();
    }
}
