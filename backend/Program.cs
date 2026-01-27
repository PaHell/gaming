using backend.Data.Repositories.Gaming;
using Backend.Configuration;
using Backend.Data;
using Backend.Data.Models.General;
using Backend.Data.Repositories.Identity;
using Backend.Services.Identity;
using Microsoft.EntityFrameworkCore;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Bind configuration
        var config = new ApplicationConfiguration();
        builder.Configuration.Bind(config);

        // Register services
        builder.Services.AddSingleton(config);
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton(new AppCache());
        builder.Services.AddSingleton<UserService>();

        // Register repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
        builder.Services.AddScoped<IUserGameScoreRepository, UserGameScoreRepository>();

        // Register hosted services
        //builder.Services.AddHostedService<StockSymbolService>();

        // Add database

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services
            .AddEndpointsApiExplorer()
            .AddControllers();
        builder.Services.AddOpenApiDocument((options) =>
        {
            options.Title = config.AppTitle;
            options.UseControllerSummaryAsTagDescription = true;
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

        app.UseCors(policy =>
        {
            policy.WithOrigins(config.CorsHosts.Split(','))
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });

        // Apply database migrations
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();

        app.Run();
    }
}