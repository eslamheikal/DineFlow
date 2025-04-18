using MediatR;
using Shared.Contracts.Auth;
using Shared.Domain.Common;

namespace AuthService.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;