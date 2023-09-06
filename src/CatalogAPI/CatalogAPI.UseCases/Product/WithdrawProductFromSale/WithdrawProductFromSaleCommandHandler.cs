using CatalogAPI.Domain.Repositories.Interfaces;
using Infrastructure.Exceptions;
using MediatR;

namespace CatalogAPI.UseCases.WithdrawFromSale;

public class WithdrawProductFromSaleCommandHandler : IRequestHandler<WithdrawProductFromSaleCommand>
{
    public readonly IUnitOfWork _db;

    public WithdrawProductFromSaleCommandHandler(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task Handle(WithdrawProductFromSaleCommand request, CancellationToken cancellationToken)
    {
        var product = _db.Products.GetById(request.Id);

        if (product == null)
        {
            throw new NotFoundException("Product with this Id was not founded", nameof(request.Id));
        }

        product.IsSale = true;
        await _db.SaveChangesAsync(cancellationToken);
    }
}
