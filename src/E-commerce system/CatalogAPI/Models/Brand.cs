using MassTransit.NewIdFormatters;
using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models
{
    public class Brand: BaseEntity
    {
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
