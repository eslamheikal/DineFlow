using AuthService.Domain.Entities;
using AuthService.Domain.Services;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data;

public static class UserSeeder
{
    public static async Task SeedUsersAsync(AuthDbContext context, IPasswordHasher passwordHasher)
    {
        if (!await context.Set<User>().AnyAsync())
        {
            var superAdminRole = await context.Set<Role>().FirstOrDefaultAsync(r => r.Name == "SuperAdmin");

            var superAdmin = User.Create(
                email: "admin@dineflow.com",
                passwordHash: passwordHasher.HashPassword("Admin"),
                firstName: "Super",
                lastName: "Admin"
            );

            superAdmin.AddRole(superAdminRole);

            await context.Set<User>().AddAsync(superAdmin);
            await context.SaveChangesAsync();
        }
    }
} 