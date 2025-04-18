using AuthService.Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<string> _hasher;

    public PasswordHasher()
    {
        _hasher = new PasswordHasher<string>();
    }

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var result = _hasher.VerifyHashedPassword(null!, hash, password);
        return result != PasswordVerificationResult.Failed;
    }
} 