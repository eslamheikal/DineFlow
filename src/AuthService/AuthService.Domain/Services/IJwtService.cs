using AuthService.Domain.Entities;

namespace AuthService.Domain.Services;

public interface IJwtService
{
    (string accessToken, string refreshToken, DateTime expiresAt) GenerateTokens(User user);
    bool ValidateToken(string token);
    int GetUserIdFromToken(string token);
    IEnumerable<string> GetRolesFromToken(string token);
    IEnumerable<string> GetPermissionsFromToken(string token);
} 