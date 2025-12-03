using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/aggregator/system")]
    public class SystemAggregatorController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SystemAggregatorController> _logger;

        public SystemAggregatorController(
            IConfiguration configuration,
            ILogger<SystemAggregatorController> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // HttpClient, який ігнорує SSL помилки для localhost
            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            })
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        [HttpGet("health")]
        public async Task<IActionResult> GetSystemHealth()
        {
            try
            {
                var gatewayUrl = _configuration["GatewayUrl"] ?? "https://localhost:7266";

                // Aggregator теж використовує Gateway!
                var catalogTask = CheckHealthThroughGatewayAsync($"{gatewayUrl}/api/catalog/health");
                var ordersTask = CheckHealthThroughGatewayAsync($"{gatewayUrl}/api/orders/health");
                var reviewsTask = CheckHealthThroughGatewayAsync($"{gatewayUrl}/api/reviews/health");
                var aggregatorTask = CheckHealthThroughGatewayAsync($"{gatewayUrl}/api/aggregator/health");

                await Task.WhenAll(catalogTask, ordersTask, reviewsTask, aggregatorTask);

                var servicesList = new List<object>
                {
                    new
                    {
                        Name = "Catalog API",
                        Status = catalogTask.Result.Status,
                        ResponseTime = catalogTask.Result.ResponseTimeMs,
                        AccessedThrough = "Gateway"
                    },
                    new
                    {
                        Name = "Orders API",
                        Status = ordersTask.Result.Status,
                        ResponseTime = ordersTask.Result.ResponseTimeMs,
                        AccessedThrough = "Gateway"
                    },
                    new
                    {
                        Name = "Review Service API",
                        Status = reviewsTask.Result.Status,
                        ResponseTime = reviewsTask.Result.ResponseTimeMs,
                        AccessedThrough = "Gateway"
                    },
                    new
                    {
                        Name = "Aggregator API",
                        Status = aggregatorTask.Result.Status,
                        ResponseTime = aggregatorTask.Result.ResponseTimeMs,
                        AccessedThrough = "Gateway"
                    }
                };

                return Ok(new
                {
                    Timestamp = DateTime.UtcNow,
                    GatewayUrl = gatewayUrl,
                    Services = servicesList,
                    AllHealthy = catalogTask.Result.Status == "Healthy" &&
                                ordersTask.Result.Status == "Healthy" &&
                                reviewsTask.Result.Status == "Healthy" &&
                                aggregatorTask.Result.Status == "Healthy",
                    Architecture = "All requests go through API Gateway"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking system health through gateway");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("gateway-test")]
        public async Task<IActionResult> TestGatewayRoutes()
        {
            var gatewayUrl = _configuration["GatewayUrl"] ?? "https://localhost:7266";
            var results = new List<object>();

            var routes = new[]
            {
                new { Route = "/api/catalog/health", Service = "Catalog API" },
                new { Route = "/api/orders/health", Service = "Orders API" },
                new { Route = "/api/reviews/health", Service = "Reviews API" },
                new { Route = "/api/aggregator/health", Service = "Aggregator API" },
                new { Route = "/api/catalog/api/categories", Service = "Catalog Categories" },
                new { Route = "/api/orders/api/orders", Service = "Orders List" },
                new { Route = "/api/reviews/api/discussions", Service = "Reviews Discussions" }
            };

            foreach (var route in routes)
            {
                try
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    var response = await _httpClient.GetAsync($"{gatewayUrl}{route.Route}");
                    stopwatch.Stop();

                    results.Add(new
                    {
                        Service = route.Service,
                        Route = route.Route,
                        FullUrl = $"{gatewayUrl}{route.Route}",
                        StatusCode = (int)response.StatusCode,
                        Success = response.IsSuccessStatusCode,
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds
                    });
                }
                catch (Exception ex)
                {
                    results.Add(new
                    {
                        Service = route.Service,
                        Route = route.Route,
                        FullUrl = $"{gatewayUrl}{route.Route}",
                        Error = ex.Message,
                        Success = false
                    });
                }
            }

            return Ok(new
            {
                Timestamp = DateTime.UtcNow,
                GatewayUrl = gatewayUrl,
                TestResults = results,
                Note = "Testing all routes through API Gateway"
            });
        }

        private async Task<(string Status, long ResponseTimeMs)> CheckHealthThroughGatewayAsync(string url)
        {
            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var response = await _httpClient.GetAsync(url);
                stopwatch.Stop();

                return response.IsSuccessStatusCode
                    ? ("Healthy", stopwatch.ElapsedMilliseconds)
                    : ($"Unhealthy ({(int)response.StatusCode})", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                return ($"Gateway Error: {ex.Message.Split(':')[0]}", 0);
            }
        }
    }

}
