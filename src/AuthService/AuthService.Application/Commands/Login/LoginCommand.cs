using AuthService.Application.Dtos;
using MediatR;
using Shared.Domain.Attributes;
using Shared.Domain.Common;

namespace AuthService.Application.Commands.Login;

public record LoginCommand : IRequest<Result<LoginResponseDto>> 
{
    public string Email { get; set; } = string.Empty;

    [IgnoreLogging]
    public string Password { get; set; } = string.Empty;
}