using MediatR;
using Shared.Domain.Attributes;
using Shared.Domain.Common;

namespace AuthService.Application.Commands.CreateUser;

public record CreateUserCommand : IRequest<Result<int>>
{
    public required string Email { get; set; } = string.Empty;
    [IgnoreLogging]
    public required string Password { get; set; } = string.Empty;
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public List<int> RoleIds { get; set; } = new();
} 