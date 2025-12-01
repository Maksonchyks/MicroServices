using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var ordersService = builder.AddProject<Orders_Api>("orders-service");
var catalogService = builder.AddProject<Catalog_Api>("catalog-service");
var reviewsService = builder.AddProject<ReviewService_API>("reviews-service");
var aggregatorService = builder.AddProject<AggregatorService>("aggregator-service");
var apiGateway = builder.AddProject<ApiGateway>("api-gateway");

builder.Build().Run();
