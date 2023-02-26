using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models
{
    public class Category: BaseEntity
    {
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
