namespace Catalog.Domain.Enteties
{
    public class ProductDetail
    {
        public Guid ProductDetailId { get; init; } = Guid.NewGuid();
        public string Description {  get; set; }
        public string Manufacter { get; set; }
        public Guid ProductId { get; init; }
        public Product? Product { get; set; }
        public ProductDetail(string description, string manufacter, Guid productId)
        {
            Description = description;
            Manufacter = manufacter;
            ProductId = productId;
        }

    }
}
