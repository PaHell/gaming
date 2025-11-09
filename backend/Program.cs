using Backend.Configuration;
using Backend.Data;
using Backend.Data.Models;
using Backend.Data.Models.General;
using Backend.Data.Repositories.Identity;
using Microsoft.EntityFrameworkCore;

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

// Add database

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument((options) =>
{
    options.Title = "Coruscant API Title";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.UseOpenApi();

app.Run();

// Apply database migrations
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
dbContext.Database.Migrate();

