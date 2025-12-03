using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorService.Controllers
{
    [ApiController]
    [Route("api/aggregator/users")]
    public class UserAggregatorController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserAggregatorController> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserAggregatorController(
            IConfiguration configuration,
            ILogger<UserAggregatorController> logger)
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

        [HttpGet("{userId}/profile")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            try
            {
                var gatewayUrl = _configuration["GatewayUrl"] ?? "https://localhost:7266";
                var reviewsGatewayUrl = $"{gatewayUrl}/api/reviews";

                var reviewsTask = GetThroughGatewayAsync<object>($"{reviewsGatewayUrl}/api/reviews/user/{userId}");
                var ratingsTask = GetThroughGatewayAsync<object>($"{reviewsGatewayUrl}/api/ratings/user/{userId}");

                await Task.WhenAll(reviewsTask, ratingsTask);

                return Ok(new
                {
                    UserId = userId,
                    Reviews = reviewsTask.Result,
                    Ratings = ratingsTask.Result,
                    AggregatedAt = DateTime.UtcNow,
                    GatewayUrl = gatewayUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting profile for user {UserId} through gateway", userId);
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
