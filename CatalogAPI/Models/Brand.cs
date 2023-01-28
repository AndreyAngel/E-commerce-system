namespace CatalogAPI.Models
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
