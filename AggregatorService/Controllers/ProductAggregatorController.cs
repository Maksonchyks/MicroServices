using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/aggregator/products")]
    public class ProductAggregatorController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductAggregatorController> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductAggregatorController(
            IConfiguration configuration,
            ILogger<ProductAggregatorController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            })
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        [HttpGet("{productId:guid}/full")]
        public async Task<IActionResult> GetProductFullDetails(Guid productId)
        {
            try
            {
                var gatewayUrl = _configuration["GatewayUrl"] ?? "https://localhost:7266";

                // Всі запити через Gateway!
                var catalogGatewayUrl = $"{gatewayUrl}/api/catalog";
                var reviewsGatewayUrl = $"{gatewayUrl}/api/reviews";

                // Паралельні запити через Gateway
                var productTask = GetThroughGatewayAsync<object>($"{catalogGatewayUrl}/api/products/{productId}");
                var imagesTask = GetThroughGatewayAsync<object>($"{catalogGatewayUrl}/api/products/{productId}/images");
                var reviewsTask = GetThroughGatewayAsync<object>($"{reviewsGatewayUrl}/api/reviews/product/{productId}");

                await Task.WhenAll(productTask, imagesTask, reviewsTask);

                return Ok(new
                {
                    Product = productTask.Result,
                    Images = imagesTask.Result,
                    Reviews = reviewsTask.Result,
                    AggregatedAt = DateTime.UtcNow,
                    GatewayUsed = gatewayUrl,
                    RequestPath = "All requests routed through API Gateway"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aggregating product {ProductId} through gateway", productId);
                return StatusCode(500, new
                {
                    Error = "Failed to aggregate product data through gateway",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { Error = "Search query is required" });

            try
            {
                var gatewayUrl = _configuration["GatewayUrl"] ?? "https://localhost:7266";
                var catalogGatewayUrl = $"{gatewayUrl}/api/catalog";
                var reviewsGatewayUrl = $"{gatewayUrl}/api/reviews";

                var productsTask = GetThroughGatewayAsync<object>($"{catalogGatewayUrl}/api/products/search?searchTerm={Uri.EscapeDataString(query)}");
                var reviewsTask = GetThroughGatewayAsync<object>($"{reviewsGatewayUrl}/api/reviews/product/search?query={Uri.EscapeDataString(query)}");

                await Task.WhenAll(productsTask, reviewsTask);

                return Ok(new
                {
                    Query = query,
                    Products = productsTask.Result,
                    Reviews = reviewsTask.Result,
                    AggregatedAt = DateTime.UtcNow,
                    GatewayUrl = gatewayUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        private async Task<T?> GetThroughGatewayAsync<T>(string url)
        {
            try
            {
                _logger.LogInformation("Calling through Gateway: {Url}", url);
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Gateway call failed: {Url}", url);
                return default;
            }
        }
    }
}
