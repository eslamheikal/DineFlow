using Shared.Domain.Common;
using Shared.Domain.ValueObjects;

namespace AuthService.Domain.Entities;

public class User : BaseEntity
{
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public List<Role> Roles { get; private set; } = new();

    private User() { } // For EF Core

    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName)
    {
        return new User
        {
            Email = Email.Create(email),
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            IsActive = true,
            Roles = new List<Role>(),
        };
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void AddRole(Role role)
    {
        if (!Roles.Contains(role))
        {
            Roles.Add(role);
        }
    }

    public void RemoveRole(Role role)
    {
        if (Roles.Contains(role))
        {
            Roles.Remove(role);
        }
    }
}