namespace OrderAPI.Contracts.DTO.Order;

/// <summary>
/// The order product data transfer object
/// </summary>
public class OrderProductDTO
{
    /// <summary>
    /// Order Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of order products that match a product from the catalog 
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Total value of order products that match a product from the catalog
    /// </summary>
    public double TotalValue { get; set; }
}
