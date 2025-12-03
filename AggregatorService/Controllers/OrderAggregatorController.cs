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
                var gatewayUrl = _configuration["GatewayUrl"] ?? "https://localhost:7266";
                var ordersGatewayUrl = $"{gatewayUrl}/api/orders";

                var order = await GetThroughGatewayAsync<object>($"{ordersGatewayUrl}/api/orders/{orderId}");

                if (order == null)
                    return NotFound(new
                    {
                        OrderId = orderId,
                        Message = "Order not found",
                        GatewayUrl = ordersGatewayUrl
                    });

                return Ok(new
                {
                    Order = order,
                    AggregatedAt = DateTime.UtcNow,
                    GatewayUrl = gatewayUrl,
                    Note = "Retrieved through API Gateway"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order {OrderId} through gateway", orderId);
                return StatusCode(500, new
                {
                    Error = "Failed to get order details through gateway",
                    OrderId = orderId,
                    Details = ex.Message
                });
            }
        }

        [HttpGet("customer/{customerName}")]
        public async Task<IActionResult> GetCustomerOrders(string customerName)
        {
            try
            {
                var gatewayUrl = _configuration["GatewayUrl"] ?? "https://localhost:7266";
                var ordersGatewayUrl = $"{gatewayUrl}/api/orders";

                var orders = await GetThroughGatewayAsync<List<object>>($"{ordersGatewayUrl}/api/orders");

                var customerOrders = orders?
                    .Where(o => JsonSerializer.Serialize(o).Contains($"\"CustomerName\":\"{customerName}\"", StringComparison.OrdinalIgnoreCase))
                    .ToList() ?? new List<object>();

                return Ok(new
                {
                    CustomerName = customerName,
                    Orders = customerOrders,
                    TotalOrders = customerOrders.Count,
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
