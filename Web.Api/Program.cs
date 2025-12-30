using Application.Handler.Products;
using Application.Handler.Sales;
using Application.Interfaces.Repositories;
using Application.Services;
using Application.UseCases.Products;
using Application.UseCases.Sales;
using Application.Validators;
using FluentValidation;
using Infraestructure.Data;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Web.Api.Configuration;
using Web.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PosDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Add configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<ProductRequestValidator>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductsUseCase, ProductsHandler>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<ISalesUseCase, SalesHandler>();
builder.Services.AddScoped<PaymentService>();

// Validate configuration on startup
if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("DefaultConnection")))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured properly.");
}

var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrWhiteSpace(jwtSecretKey) || jwtSecretKey.Length < 32)
{
    throw new InvalidOperationException("JWT Secret key must be at least 32 characters long.");
}


// Configure CORS for frontend integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("https://localhost:7267", "http://localhost:7267")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
