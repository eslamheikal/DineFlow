using AuthService.Application.Dtos;
using AuthService.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Common;

namespace AuthService.Application.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserProfileResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetCurrentUserQueryHandler> _logger;

    public GetCurrentUserQueryHandler(
        IUserRepository userRepository,
        ILogger<GetCurrentUserQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<UserProfileResponseDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(r => r.Id == request.Id, [r => r.Roles]);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", request.Id);
            return Result<UserProfileResponseDto>.Failure("User not found");
        }

        var response = new UserProfileResponseDto
        {
            Id = user.Id,
            Email = user.Email.Value,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,
            Roles = user.Roles.Select(r => r.Name).ToList()
        };

        return Result<UserProfileResponseDto>.Success(response);
    }
} 