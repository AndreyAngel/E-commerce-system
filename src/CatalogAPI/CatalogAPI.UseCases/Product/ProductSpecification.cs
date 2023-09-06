using CatalogAPI.Contracts.DTO;

namespace CatalogAPI.UseCases;

public class ProductSpecification : Specification
{
    public ProductSpecification(string? sort,
                                string? searchString,
                                ProductFilterDTO? filters) : base(sort, searchString)
    {
        Filters = filters;
    }

    /// <summary>
    /// Filters
    /// </summary>
    public ProductFilterDTO? Filters { get; }
}
