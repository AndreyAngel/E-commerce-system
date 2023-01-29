using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models.DTO;

public class Product
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }
}
