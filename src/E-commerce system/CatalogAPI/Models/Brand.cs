using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name not provided")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
