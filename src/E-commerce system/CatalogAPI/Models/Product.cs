using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name not provided")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0, 999999999999, ErrorMessage = "Invalid price")]
        public double Price { get; set; }


        [Range(1, 999999999999, ErrorMessage = "Invalid CategoryId")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        [Range(1, 999999999999, ErrorMessage = "Invalid BrandId")]
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }

        public bool IsSale { get; set; } = false;
    }
}
