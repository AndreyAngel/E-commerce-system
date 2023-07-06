namespace Infrastructure.DTO;

/// <summary>
/// Products data transfer object used by RabbitMQ
/// </summary>
/// <typeparam name="Type"> Type of presentation product </typeparam>
public class ProductListDTORabbitMQ<Type>
{
    /// <summary>
    /// Products list
    /// </summary>
    public List<Type> Products { get; set; } = new List<Type>();
}
