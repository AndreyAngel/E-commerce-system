using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.GetBrandsList;
public class GetBrandListQuery : IStreamRequest<BrandDTOResponse>
{
    public Specification Specification { get; }

    // TODO: реализовать поиск и сортировку
    public GetBrandListQuery(string? sort = null, string? searchString = null)
    {
        Specification = new Specification(sort: sort, searchString: searchString);
    }
}
