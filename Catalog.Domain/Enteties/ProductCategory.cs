namespace Catalog.Domain.Enteties
{
    public class ProductCategory
    {
        public Guid ProductId { get; init; }
        public Guid CategoryId { get; init; }
        public Product? Product { get; set; }
        public Category? Category { get; set; }

        public ProductCategory(Guid productId, Guid categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
    }
}
