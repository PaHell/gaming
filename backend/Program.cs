using backend.Data.Repositories.AlphaVantage;
using backend.Libraries;
using backend.Services.Stocks;
using Backend.Configuration;
using Backend.Data;
using Backend.Data.Models;
using Backend.Data.Models.General;
using Backend.Data.Repositories.Identity;
using Microsoft.EntityFrameworkCore;
using StockSharp.Algo.Indicators;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
var config = new ApplicationConfiguration();
builder.Configuration.Bind(config);

// Register services
builder.Services.AddSingleton(config);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddSingleton(new AppCache());

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddScoped<IAlphaVantageRepository, AlphaVantageRepository>();

// Add database

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services
    .AddEndpointsApiExplorer()
    .AddControllers();
builder.Services.AddOpenApiDocument((options) =>
{
    options.Title = "Coruscant API Title";
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}

app.UseOpenApi();

app.MapControllers();

// Apply database migrations
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//dbContext.Database.Migrate();

await GenerateCSharpClient.CreateAsync("./Libraries/finnhub.json", "./Libraries/FinnhubClient.cs");

app.Run();

