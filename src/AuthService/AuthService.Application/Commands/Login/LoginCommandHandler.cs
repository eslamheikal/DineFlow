using AuthService.Application.Dtos;
using AuthService.Domain.Repositories;
using AuthService.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Common;
using Shared.Domain.Repositories;
using Shared.Infrastructure.Logging;

namespace AuthService.Application.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly StructuredLogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        ILogger<LoginCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _logger = new StructuredLogger<LoginCommandHandler>(logger, "AuthService");
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _logger.LogOperationAsync("Login", async () =>
        {
            // Get user by email
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<LoginResponseDto>.Failure("Invalid email or password");
            }

            // Verify password
            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Result<LoginResponseDto>.Failure("Invalid email or password");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return Result<LoginResponseDto>.Failure("User account is deactivated");
            }

            // Update last login
            user.UpdateLastLogin();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Generate tokens
            var (accessToken, refreshToken, expiresAt) = _jwtService.GenerateTokens(user);

            // Create response
            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                Email = user.Email.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.Roles.Select(r => r.Name).ToList(),
                Permissions = user.Roles
                    .SelectMany(r => r.Permissions)
                    .Select(p => $"{p.Resource}:{p.Action}")
                    .Distinct()
                    .ToList()
            };

            return Result<LoginResponseDto>.Success(response);
        });
    }
} 