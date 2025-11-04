using Catalog.Application.Interfaces;
using Catalog.Application.Services;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories.Implementations;
using Catalog.Infrastructure.Repositories.Interfaces;
using Catalog.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Catalog.Application.Services.Interfaces;
using Catalog.Application.Mapper;
using Catalog.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    options.UseMySql(connectionString, serverVersion, mySqlOptions =>
    {
        mySqlOptions.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName);
    });
});

// Repositories & Unit of Work
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Database seeding
using (var scope = app.Services.CreateScope())
{
    try
    {
        await DbInitializer.InitializeAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database seeding");
    }
}

app.Run();