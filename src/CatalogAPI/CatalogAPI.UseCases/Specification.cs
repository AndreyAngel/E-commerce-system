namespace CatalogAPI.UseCases;

public class Specification
{
    public Specification(string? sort, string? searchString)
    {
        Sort = sort;
        SearchString = searchString;
    }

    /// <summary>
    /// Sort lambda
    /// </summary>
    public string? Sort { get; }

    /// <summary>
    /// Search string
    /// </summary>
    public string? SearchString { get; }
}
