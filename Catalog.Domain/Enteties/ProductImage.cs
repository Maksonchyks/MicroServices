namespace Catalog.Domain.Enteties
{
    public class ProductImage
    {
        public Guid ProductImageId { get; init; } = Guid.NewGuid();
        public string Url { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public ProductImage(string url, Guid productId)
        {
            Url = url;
            ProductId = productId;
        }
    }
}
