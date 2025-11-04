using Catalog.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Catalog.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = CreateProblemDetails(context, statusCode, exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.Conflict,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

        private object CreateProblemDetails(HttpContext context, int statusCode, Exception exception)
        {
            var problemDetails = new
            {
                Type = GetProblemType(statusCode),
                Title = GetProblemTitle(statusCode),
                Status = statusCode,
                Detail = exception.Message,
                Instance = context.Request.Path,
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier,
                // Додаткові деталі тільки в розробці
                Errors = _env.IsDevelopment() ? GetErrors(exception) : null,
                StackTrace = _env.IsDevelopment() ? exception.StackTrace : null
            };

            return problemDetails;
        }

        private static object? GetErrors(Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                return new { ValidationErrors = validationException.Message };
            }
            return null;
        }

        private static string GetProblemType(int statusCode) =>
            statusCode switch
            {
                400 => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                401 => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                404 => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                409 => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                500 => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                _ => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            };

        private static string GetProblemTitle(int statusCode) =>
            statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not Found",
                409 => "Conflict",
                500 => "Internal Server Error",
                _ => "An error occurred"
            };
    }
}