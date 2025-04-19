using AuthService.Application;
using AuthService.Application.Services;
using AuthService.Domain.Repositories;
using AuthService.Domain.Services;
using AuthService.Infrastructure.Context;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.Common;
using Shared.Domain.Repositories;
using Shared.Infrastructure.Logging;
using System.Text;
using System.Text.Json;

namespace AuthService.API.Extensions;

public static class SetupExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        var Configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
     
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured")))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var json = JsonSerializer.Serialize(ApiResponse<bool>.Fail("Invalid or expired token. Please log in again."));
                        context.Response.WriteAsync(json);

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        // This will cause the exception to be thrown
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }

    public static IServiceCollection AddAppContext(this IServiceCollection services)
    {
        var Configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
            options => options.MigrationsHistoryTable("__EFMigrationsHistory", AuthDbContext.Schema)));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AuthApplication).Assembly);
        });

        return services;
    }

    public static IServiceCollection AddPipelineBehavior(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}