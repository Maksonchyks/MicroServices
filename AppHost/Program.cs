using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// 1. MySQL контейнери
var mysqlOrders = builder.AddContainer("mysql-orders", "mysql", "8.0")
    .WithVolume("mysql-orders-data", "/var/lib/mysql")
    .WithEnvironment("MYSQL_ROOT_PASSWORD", "1234")
    .WithEnvironment("MYSQL_DATABASE", "ordersdb")
    .WithHttpEndpoint(3306, 3306);  // Для контейнерів працює WithHttpEndpoint

var mysqlCatalog = builder.AddContainer("mysql-catalog", "mysql", "8.0")
    .WithVolume("mysql-catalog-data", "/var/lib/mysql")
    .WithEnvironment("MYSQL_ROOT_PASSWORD", "1234")
    .WithEnvironment("MYSQL_DATABASE", "catalogdb")
    .WithHttpEndpoint(3307, 3306);

// 2. PostgreSQL контейнер
var postgresReviews = builder.AddContainer("postgres-reviews", "postgres", "15")
    .WithVolume("postgres-reviews-data", "/var/lib/postgresql/data")
    .WithEnvironment("POSTGRES_PASSWORD", "1234")
    .WithEnvironment("POSTGRES_DB", "reviewsdb")
    .WithHttpEndpoint(5432, 5432);

// 3. Сервіси
var ordersService = builder.AddProject<Orders_Api>("orders-service");
var catalogService = builder.AddProject<Catalog_Api>("catalog-service");
var reviewsService = builder.AddProject<ReviewService_API>("reviews-service");
var aggregatorService = builder.AddProject<AggregatorService>("aggregator-service");
var apiGateway = builder.AddProject<ApiGateway>("api-gateway");

builder.Build().Run();
