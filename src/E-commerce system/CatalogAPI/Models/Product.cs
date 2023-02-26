using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models
{
    public class Product: BaseEntity
    {
        [Range(0, 999999999999, ErrorMessage = "Invalid price")]
        public double Price { get; set; }


        [Range(1, 999999999999, ErrorMessage = "Invalid CategoryId")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }


        [Range(1, 999999999999, ErrorMessage = "Invalid BrandId")]
        public int BrandId { get; set; }
        public virtual Brand? Brand { get; set; }

        public bool IsSale { get; set; } = false;
    }
}
