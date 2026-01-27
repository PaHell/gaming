using backend.Data.Repositories.Gaming;
using Backend.Attributes.Identity;
using Backend.Configuration;
using Backend.Data;
using Backend.Data.Models.General;
using Backend.Data.Models.Identity;
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

        // Validate JWT secret key
        if (string.IsNullOrEmpty(config.Token.SecretKey) || config.Token.SecretKey.Length < 32)
        {
            throw new InvalidOperationException("Token.SecretKey must be at least 32 characters long for security.");
        }

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
                // Small clock skew to handle minor clock differences between servers
                ClockSkew = TimeSpan.FromMinutes(1),
                // Set custom claim types to match JWT token structure
                NameClaimType = UserClaims.UserId,
                RoleClaimType = UserClaims.Role
            };

            // Configure cookie and header authentication for JWT
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // First check for token in Authorization header (standard approach)
                    var authHeader = context.Request.Headers.Authorization.ToString();
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = authHeader.Substring("Bearer ".Length).Trim();
                    }
                    // Fall back to cookie-based token if not in header
                    else
                    {
                        var accessToken = context.Request.Cookies[CookieConfiguration.AccessTokenName];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                    }
                    return Task.CompletedTask;
                }
            };

            // Disable JWT claim type mapping to prevent automatic transformation
            options.MapInboundClaims = false;
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

        // CORS must be called early in the pipeline
        app.UseCors(policy =>
        {
            var corsHosts = config.CorsHosts.Split(',');
            if (corsHosts.Contains("*"))
            {
                // Allow any origin when wildcard is specified, but can't use credentials
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }
            else
            {
                // Specific origins can use credentials
                policy.WithOrigins(corsHosts)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            }
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Apply database migrations
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();

        app.Run();
    }
}