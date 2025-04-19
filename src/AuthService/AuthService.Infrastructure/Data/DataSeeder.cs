using AuthService.Domain.Services;
using AuthService.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Data;

public static class DataSeeder
{

    public static async Task AddDataSeeder(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var context = provider.GetRequiredService<AuthDbContext>();
            var passwordHasher = provider.GetRequiredService<IPasswordHasher>();
            await SeedDataAsync(context, passwordHasher);
        }
    }

    public static async Task SeedDataAsync(AuthDbContext context, IPasswordHasher passwordHasher)
    {
        // Seed in order: Permissions -> Roles
        await PermissionSeeder.SeedPermissionsAsync(context);
        await RoleSeeder.SeedRolesAsync(context);
        await UserSeeder.SeedUsersAsync(context, passwordHasher);
    }
} 