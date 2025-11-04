namespace Catalog.Domain.Enteties
{
    public class Product
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductDetail? ProductDetail { get; set; } 
        public List<ProductImage> ProductImages { get; set; } = new();
        public List<ProductCategory> ProductCategories { get; set; } = new();

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}
