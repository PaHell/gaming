using Microsoft.EntityFrameworkCore;
using Backend.Configuration;
using Backend.Data;
using Backend.Data.Models.General;
using Backend.Data.Repositories.Identity;
using backend.Data.Repositories.Stocks;
using backend.Libraries;
using backend.Services.Stocks;
using backend.Data.Repositories.Companies;
using backend.Data.Repositories;

public partial class Program
{
    public static readonly string FINNHUB_HTTP_CLIENT = "Finnhub";

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
        builder.Services.AddHttpClient(FINNHUB_HTTP_CLIENT, configure =>
        {
            configure.DefaultRequestHeaders.Add("X-Finnhub-Token", config.FinnhubApiKey);
        });
        builder.Services.AddSingleton(new AppCache());

        // Register repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICountryRepository, CountryRepository>();
        builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
        builder.Services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
        builder.Services.AddScoped<IStockMarketNewsRepository, StockMarketNewsRepository>();
        builder.Services.AddScoped<IStockPriceRepository, StockPriceRepository>();
        builder.Services.AddScoped<IStockSymbolRepository, StockSymbolRepository>();

        // Register hosted services
        builder.Services.AddHostedService<StockSymbolService>();

        // Add database

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services
            .AddEndpointsApiExplorer()
            .AddControllers();
        builder.Services.AddOpenApiDocument((options) =>
        {
            options.Title = "Volatility API";
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
        //dbContext.Database.Migrate();

        await GenerateCSharpClient.CreateAsync("./Libraries/finnhub.json", "./Libraries/FinnhubClient.cs");

        app.Run();
    }
}