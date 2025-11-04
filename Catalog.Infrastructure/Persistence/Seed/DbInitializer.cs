using Catalog.Domain.Enteties;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<CatalogDbContext>();
                // Використовуємо ILoggerFactory замість ILogger<DbInitializer>
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("DbInitializer");

                // Застосовуємо міграції
                await context.Database.MigrateAsync();

                await SeedCategoriesAsync(context, logger);
                await SeedProductsAsync(context, logger);
                await SeedProductDetailsAsync(context, logger);
                await SeedProductImagesAsync(context, logger);
                await SeedProductCategoriesAsync(context, logger);

                logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("DbInitializer");
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private static async Task SeedCategoriesAsync(CatalogDbContext context, ILogger logger)
        {
            if (await context.Categories.AnyAsync())
            {
                logger.LogInformation("Categories already exist - skipping seed");
                return;
            }

            var categories = new List<Category>
            {
                new Category("Electronics"),
                new Category("Books"),
                new Category("Clothing"),
                new Category("Home & Garden"),
                new Category("Sports")
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} categories", categories.Count);
        }

        private static async Task SeedProductsAsync(CatalogDbContext context, ILogger logger)
        {
            if (await context.Products.AnyAsync())
            {
                logger.LogInformation("Products already exist - skipping seed");
                return;
            }

            var products = new List<Product>
            {
                new Product("iPhone 15", 999.99m),
                new Product("Samsung Galaxy S24", 899.99m),
                new Product("MacBook Pro", 2499.99m),
                new Product("The Great Gatsby", 12.99m),
                new Product("Programming C#", 45.99m),
                new Product("Nike Air Max", 129.99m),
                new Product("Adidas Ultraboost", 149.99m)
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} products", products.Count);
        }

        private static async Task SeedProductDetailsAsync(CatalogDbContext context, ILogger logger)
        {
            if (await context.ProductDetails.AnyAsync())
            {
                logger.LogInformation("Product details already exist - skipping seed");
                return;
            }

            var products = await context.Products.ToListAsync();
            var productDetails = new List<ProductDetail>
            {
                new ProductDetail(
                    "Latest iPhone with advanced camera system",
                    "Apple",
                    products[0].Id
                ),
                new ProductDetail(
                    "Powerful Android smartphone with excellent display",
                    "Samsung",
                    products[1].Id
                ),
                new ProductDetail(
                    "Professional laptop for developers and designers",
                    "Apple",
                    products[2].Id
                ),
                new ProductDetail(
                    "Classic American novel by F. Scott Fitzgerald",
                    "Scribner",
                    products[3].Id
                )
            };

            await context.ProductDetails.AddRangeAsync(productDetails);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} product details", productDetails.Count);
        }

        private static async Task SeedProductImagesAsync(CatalogDbContext context, ILogger logger)
        {
            if (await context.ProductImages.AnyAsync())
            {
                logger.LogInformation("Product images already exist - skipping seed");
                return;
            }

            var products = await context.Products.ToListAsync();
            var productImages = new List<ProductImage>
            {
                new ProductImage("https://example.com/images/iphone15-1.jpg", products[0].Id),
                new ProductImage("https://example.com/images/iphone15-2.jpg", products[0].Id),
                new ProductImage("https://example.com/images/galaxy-s24.jpg", products[1].Id),
                new ProductImage("https://example.com/images/macbook-pro.jpg", products[2].Id),
                new ProductImage("https://example.com/images/great-gatsby.jpg", products[3].Id)
            };

            await context.ProductImages.AddRangeAsync(productImages);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} product images", productImages.Count);
        }

        private static async Task SeedProductCategoriesAsync(CatalogDbContext context, ILogger logger)
        {
            if (await context.ProductCategories.AnyAsync())
            {
                logger.LogInformation("Product categories already exist - skipping seed");
                return;
            }

            var categories = await context.Categories.ToListAsync();
            var products = await context.Products.ToListAsync();

            var productCategories = new List<ProductCategory>
            {
                new ProductCategory(products[0].Id, categories[0].CategoryId), // iPhone -> Electronics
                new ProductCategory(products[1].Id, categories[0].CategoryId), // Samsung -> Electronics
                new ProductCategory(products[2].Id, categories[0].CategoryId), // MacBook -> Electronics
                new ProductCategory(products[3].Id, categories[1].CategoryId), // Gatsby -> Books
                new ProductCategory(products[4].Id, categories[1].CategoryId), // C# Book -> Books
                new ProductCategory(products[5].Id, categories[2].CategoryId), // Nike -> Clothing
                new ProductCategory(products[6].Id, categories[2].CategoryId), // Adidas -> Clothing
                new ProductCategory(products[5].Id, categories[4].CategoryId), // Nike -> Sports
                new ProductCategory(products[6].Id, categories[4].CategoryId)  // Adidas -> Sports
            };

            await context.ProductCategories.AddRangeAsync(productCategories);
            await context.SaveChangesAsync();

            logger.LogInformation("Seeded {Count} product-category relations", productCategories.Count);
        }
    }
}