using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.DataBase.Entities;

public class OrderProduct
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; }

    public Guid? OrderId { get; set; }

    public Order? Order { get; set; }
}
