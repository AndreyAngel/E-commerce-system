using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.GetCategoriesList;
public class GetCategoryListQuery : IStreamRequest<CategoryDTOResponse>
{
    public Specification Specification { get; set; }

    // TODO: реализовать поиск и сортировку
    public GetCategoryListQuery(string? sort = null, string? searchString = null)
    {
        Specification = new Specification(sort: sort, searchString: searchString);
    }
}
