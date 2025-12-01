using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/aggregator/orders")]
    public class OrderAggregatorController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderAggregatorController> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public OrderAggregatorController(
            IConfiguration configuration,
            ILogger<OrderAggregatorController> logger)
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

        [HttpGet("{orderId:int}/details")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                var ordersUrl = _configuration["Services:OrdersService"] ?? "https://localhost:7148";
                var order = await GetFromApiAsync<object>($"{ordersUrl}/api/orders/{orderId}");

                if (order == null)
                    return NotFound(new { OrderId = orderId, Message = "Order not found" });

                return Ok(new
                {
                    Order = order,
                    AggregatedAt = DateTime.UtcNow,
                    SourceUrl = ordersUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order {OrderId}", orderId);
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("customer/{customerName}")]
        public async Task<IActionResult> GetCustomerOrders(string customerName)
        {
            try
            {
                var ordersUrl = _configuration["Services:OrdersService"] ?? "https://localhost:7148";
                var orders = await GetFromApiAsync<List<object>>($"{ordersUrl}/api/orders");

                var customerOrders = orders?
                    .Where(o => JsonSerializer.Serialize(o).Contains($"\"CustomerName\":\"{customerName}\"", StringComparison.OrdinalIgnoreCase))
                    .ToList() ?? new List<object>();

                return Ok(new
                {
                    CustomerName = customerName,
                    Orders = customerOrders,
                    TotalOrders = customerOrders.Count,
                    AggregatedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        private async Task<T?> GetFromApiAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to call API: {Url}", url);
                return default;
            }
        }
    }

}
