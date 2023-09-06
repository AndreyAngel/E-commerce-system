using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.GetProductList;

public class GetProductListQuery : IStreamRequest<ProductListDTOResponse>
{
    public ProductSpecification Specification { get; private set; }

    public GetProductListQuery(string? sort, string? searchString, ProductFilterDTO? filters)
    {
        Specification = new ProductSpecification(sort, searchString, filters);
    }
}
