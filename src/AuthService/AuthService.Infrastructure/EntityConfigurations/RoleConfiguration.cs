using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.EntityConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(255);

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.ToTable("RolePermissions"));
    }
} 