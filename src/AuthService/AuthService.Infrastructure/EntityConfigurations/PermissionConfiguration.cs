using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Resource)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Action)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(255);

        builder.HasIndex(e => new { e.Resource, e.Action })
            .IsUnique();
    }
}