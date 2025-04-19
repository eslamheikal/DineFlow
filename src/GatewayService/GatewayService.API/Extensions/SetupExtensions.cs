using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GatewayService.API.Extensions;

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
            });

        // Add after AddAuthentication()
        services.AddAuthorization(options =>
        {
            options.AddPolicy("require-auth", policy =>
                policy.RequireAuthenticatedUser());
        });

        return services;
    }
}