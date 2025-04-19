using AuthService.Application.Dtos;
using MediatR;
using Shared.Domain.Common;

namespace AuthService.Application.Queries.GetCurrentUser;

public record GetCurrentUserQuery(int Id) : IRequest<Result<UserProfileResponseDto>>
{
} 