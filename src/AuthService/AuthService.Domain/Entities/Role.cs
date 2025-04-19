using Shared.Domain.Common;

namespace AuthService.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public List<Permission> Permissions { get; private set; } = new();
    public List<User> Users { get; private set; } = new();

    private Role() { } // For EF Core

    public static Role Create(string name, string? description = null)
    {
        return new Role
        {
            Name = name,
            Description = description,
            Permissions = new(),
        };
    }

    public static Role Create(string name, string? description = null, List<Permission>? permissions = null)
    {
        return new Role
        {
            Name = name,
            Description = description,
            Permissions = permissions ?? new(),
        };
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public void AddPermission(Permission permission)
    {
        if (!Permissions.Contains(permission))
        {
            Permissions.Add(permission);
        }
    }

    public void RemovePermission(Permission permission)
    {
        if (Permissions.Contains(permission))
        {
            Permissions.Remove(permission);
        }
    }
} 