namespace GatewayAPI.DataBase;

/// <summary>
/// Entity storing token information
/// </summary>
public class Token
{
    /// <summary>
    /// Gets or set a Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or set a token value
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Gets or set a token status:
    /// true, if active
    /// false, if not active
    /// </summary>
    public bool IsActive { get; set; }
}
