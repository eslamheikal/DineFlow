using Shared.Domain.Common;

namespace AuthService.Domain.Entities;

public class Permission : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Resource { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public List<Role> Roles { get; private set; } = new();

    private Permission() { } // For EF Core

    public static Permission Create(string name, string resource, string action, string? description = null)
    {
        return new Permission
        {
            Name = name,
            Description = description,
            Resource = resource,
            Action = action,
        };
    }

    public void Update(string name, string resource, string action, string? description = null)
    {
        Name = name;
        Description = description;
        Resource = resource;
        Action = action;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Permission other)
            return false;

        return Name == other.Name &&
               Resource == other.Resource &&
               Action == other.Action;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Resource, Action);
    }
} 