namespace Catalog.Domain.Enteties
{
    public class Category
    {
        public Guid CategoryId { get; init; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<ProductCategory> ProductCategories { get; set; } = new();
        public Category(string name) 
        {
            Name = name;
        }
    }
}
