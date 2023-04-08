using OrderAPI.Models;

namespace OrderAPI.Services.Interfaces;

public interface ICartProductService : IDisposable
{
    Task<CartProductDomainModel> Create(CartProductDomainModel cartProduct);

    Task<CartProductDomainModel> Update(CartProductDomainModel cartProduct);

    Task Delete(Guid id);
}
