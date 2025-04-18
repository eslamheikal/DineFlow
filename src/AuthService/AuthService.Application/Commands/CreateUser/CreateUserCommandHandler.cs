using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Common;
using Shared.Domain.Repositories;
using Shared.Infrastructure.Logging;

namespace AuthService.Application.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly StructuredLogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        ILogger<CreateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _logger = new StructuredLogger<CreateUserCommandHandler>(logger, "AuthService");
    }

    public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await _logger.LogOperationAsync("CreateUser", async () =>
        {
            // Check if user already exists
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                return Result<int>.Failure($"User with email {request.Email} already exists");
            }

            // Hash password
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // Create user
            var user = User.Create(
                request.Email,
                passwordHash,
                request.FirstName,
                request.LastName);

            // Add roles if specified
            if (request.RoleNames.Any())
            {
                var roles = await _roleRepository.GetByNamesAsync(request.RoleNames);
                foreach (var role in roles)
                {
                    user.AddRole(role);
                }
            }

            // Save user
            _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(user.Id);
        });
    }
} 