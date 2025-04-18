using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AuthService.Infrastructure.Context;

public class AuthDbContext : DbContext
{
    public static string Schema => "auth";
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
} 