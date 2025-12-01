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
        private readonly JsonSerializerOptions _jsonOptions;

        public SystemAggregatorController(
            IConfiguration configuration,
            ILogger<SystemAggregatorController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

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
                var catalogUrl = _configuration["Services:CatalogService"] ?? "https://localhost:7267";
                var ordersUrl = _configuration["Services:OrdersService"] ?? "https://localhost:7148";
                var reviewUrl = _configuration["Services:ReviewService"] ?? "https://localhost:7097";

                var catalogTask = CheckHealthAsync($"{catalogUrl}/health");
                var ordersTask = CheckHealthAsync($"{ordersUrl}/health");
                var reviewTask = CheckHealthAsync($"{reviewUrl}/health");

                await Task.WhenAll(catalogTask, ordersTask, reviewTask);

                // Створюємо список об'єктів явно
                var servicesList = new List<object>
                {
                    new
                    {
                        Name = "Catalog API",
                        Status = catalogTask.Result.Status,
                        ResponseTime = catalogTask.Result.ResponseTimeMs,
                        Url = catalogUrl
                    },
                    new
                    {
                        Name = "Orders API",
                        Status = ordersTask.Result.Status,
                        ResponseTime = ordersTask.Result.ResponseTimeMs,
                        Url = ordersUrl
                    },
                    new
                    {
                        Name = "Review Service API",
                        Status = reviewTask.Result.Status,
                        ResponseTime = reviewTask.Result.ResponseTimeMs,
                        Url = reviewUrl
                    },
                    new
                    {
                        Name = "Aggregator API",
                        Status = "Healthy",
                        ResponseTime = 0,
                        Url = "https://localhost:7118"
                    }
                };

                return Ok(new
                {
                    Timestamp = DateTime.UtcNow,
                    Services = servicesList,  // Використовуємо список замість масиву
                    AllHealthy = catalogTask.Result.Status == "Healthy" &&
                                ordersTask.Result.Status == "Healthy" &&
                                reviewTask.Result.Status == "Healthy"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking system health");
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestEndpoints()
        {
            var results = new List<object>();

            var endpoints = new[]
            {
                new { Service = "Catalog", Url = "https://localhost:7267/health" },
                new { Service = "Orders", Url = "https://localhost:7148/health" },
                new { Service = "Reviews", Url = "https://localhost:7097/health" },
                new { Service = "Aggregator", Url = "https://localhost:7118/health" }
            };

            foreach (var endpoint in endpoints)
            {
                try
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    var response = await _httpClient.GetAsync(endpoint.Url);
                    stopwatch.Stop();

                    var content = await response.Content.ReadAsStringAsync();

                    results.Add(new
                    {
                        Service = endpoint.Service,
                        Url = endpoint.Url,
                        StatusCode = (int)response.StatusCode,
                        Success = response.IsSuccessStatusCode,
                        ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                        Response = content
                    });
                }
                catch (Exception ex)
                {
                    results.Add(new
                    {
                        Service = endpoint.Service,
                        Url = endpoint.Url,
                        Error = ex.Message,
                        Success = false,
                        ResponseTimeMs = 0
                    });
                }
            }

            return Ok(new
            {
                Timestamp = DateTime.UtcNow,
                Results = results
            });
        }

        private async Task<(string Status, long ResponseTimeMs)> CheckHealthAsync(string url)
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
                return ($"Error: {ex.Message.Split(':')[0]}", 0);
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
            catch
            {
                return default;
            }
        }
    }

}
