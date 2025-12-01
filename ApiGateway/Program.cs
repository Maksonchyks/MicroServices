var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();  // Підключаємо наш ServiceDefaults

// Додаємо YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapDefaultEndpoints();  // Health checks + CorrelationId

app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault()
                       ?? Guid.NewGuid().ToString();

    // Додаємо до всіх відповідей
    context.Response.Headers["X-Correlation-Id"] = correlationId;

    // Додаємо до всіх запитів до мікросервісів
    context.Items["X-Correlation-Id"] = correlationId;

    await next();
});

app.MapReverseProxy();  // Увімкнути проксі

app.Run();