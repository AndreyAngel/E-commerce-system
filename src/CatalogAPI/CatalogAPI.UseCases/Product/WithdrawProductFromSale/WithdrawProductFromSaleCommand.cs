using MediatR;

namespace CatalogAPI.UseCases.WithdrawFromSale;

public class WithdrawProductFromSaleCommand : IRequest
{
    public Guid Id { get; }

    public WithdrawProductFromSaleCommand(Guid id)
    {
        Id = id;
    }
}
