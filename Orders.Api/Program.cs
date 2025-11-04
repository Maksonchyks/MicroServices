using System.Data;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Orders.Bll.Interfaces;
using Orders.Bll.Mappers;
using Orders.Bll.Services;
using Orders.Dal.Interfaces;
using Orders.Dal.UOW;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddScoped<IUnitOfWork>(sp =>
{
    return new UnitOfWork(connectionString);
});

builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderStatusHistoryService, OrderStatusHistoryService>();
builder.Services.AddScoped<IPaymentRecordService, PaymentRecordService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
