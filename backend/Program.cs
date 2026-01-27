using backend.Data.Repositories.Gaming;
using Backend.Attributes.Identity;
using Backend.Configuration;
using Backend.Data;
using Backend.Data.Models.General;
using Backend.Data.Repositories.Identity;
using Backend.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        // Configure JWT Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.Token.Issuer,
                ValidAudience = config.Token.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Token.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };

            // Configure cookie authentication for JWT
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Read token from cookie
                    var accessToken = context.Request.Cookies[CookieConfiguration.AccessTokenName];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        // Configure Authorization
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, ApplicationPermissionPolicyProvider>();
        builder.Services.AddSingleton<IAuthorizationHandler, ApplicationPermissionHandler>();

        // Add database

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services
            .AddEndpointsApiExplorer()
            .AddControllers(options =>
            {
                // Add global authorization filter
                options.Filters.Add<ApplicationPermissionFilter>();
            });
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

        app.UseAuthentication();
        app.UseAuthorization();

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