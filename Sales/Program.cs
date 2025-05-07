using Sales.Core.Domain.Entities;
using Sales.Core.Domain.Interfaces;
using Sales.Core.Domain.Services;
using Sales.Core.Infra.Interfaces;
using Sales.Core.Infra.Repositories;
using Sales.Core.Infra.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

// Registro do repositório
builder.Services.AddSingleton<ISaleRepository<Sale>, SaleRepository<Sale>>();



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
