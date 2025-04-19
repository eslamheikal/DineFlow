using AuthService.Application.Commands.CreateUser;
using AuthService.Application.Commands.Login;
using AuthService.Application.Dtos;
using AuthService.Application.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Common;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<bool>>> Register([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(ApiResponse<bool>.Success(true))
            : BadRequest(ApiResponse<bool>.Fail(result.Error, result.ValidationErrors));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess 
            ? Ok(ApiResponse<LoginResponseDto>.Success(result.Value))
            : BadRequest(ApiResponse<LoginResponseDto>.Fail(result.Error, result.ValidationErrors));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserProfileResponseDto>>> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized(ApiResponse<UserProfileResponseDto>.Fail("Invalid token"));
        }

        var result = await _mediator.Send(new GetCurrentUserQuery(userId));

        return result.IsSuccess
            ? Ok(ApiResponse<UserProfileResponseDto>.Success(result.Value))
            : NotFound(ApiResponse<UserProfileResponseDto>.Fail(result.Error, result.ValidationErrors));
    }

    //[HttpPost("change-password")]
    //[Authorize]
    //public async Task<ActionResult<ApiResponse<bool>>> ChangePassword([FromBody] ChangePasswordRequest request)
    //{
    //    try
    //    {
    //        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    //        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
    //        {
    //            return Unauthorized(new ApiResponse<bool>
    //            {
    //                Success = false,
    //                Message = "Invalid token"
    //            });
    //        }

    //        var user = await _userRepository.GetByIdAsync(userId);
    //        if (user == null)
    //        {
    //            return NotFound(new ApiResponse<bool>
    //            {
    //                Success = false,
    //                Message = "User not found"
    //            });
    //        }

    //        if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
    //        {
    //            return BadRequest(new ApiResponse<bool>
    //            {
    //                Success = false,
    //                Message = "Current password is incorrect"
    //            });
    //        }

    //        user.UpdatePassword(_passwordHasher.HashPassword(request.NewPassword));
    //        await _userRepository.UpdateAsync(user);

    //        return Ok(new ApiResponse<bool>
    //        {
    //            Success = true,
    //            Data = true,
    //            Message = "Password changed successfully"
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error occurred while changing password");
    //        return StatusCode(500, new ApiResponse<bool>
    //        {
    //            Success = false,
    //            Message = "An error occurred while processing your request"
    //        });
    //    }
    //}

    //[HttpPost("refresh-token")]
    //public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    //{
    //    try
    //    {
    //        if (!_jwtService.ValidateToken(request.AccessToken))
    //        {
    //            return BadRequest(new ApiResponse<LoginResponse>
    //            {
    //                Success = false,
    //                Message = "Invalid access token"
    //            });
    //        }

    //        var userId = _jwtService.GetUserIdFromToken(request.AccessToken);
    //        var user = await _userRepository.GetByIdAsync(userId);

    //        if (user == null)
    //        {
    //            return NotFound(new ApiResponse<LoginResponse>
    //            {
    //                Success = false,
    //                Message = "User not found"
    //            });
    //        }

    //        var (accessToken, refreshToken, expiresAt) = _jwtService.GenerateTokens(user);

    //        var response = new LoginResponse
    //        {
    //            AccessToken = accessToken,
    //            RefreshToken = refreshToken,
    //            ExpiresAt = expiresAt,
    //            Email = user.Email.Value,
    //            FirstName = user.FirstName,
    //            LastName = user.LastName,
    //            Roles = user.Roles.Select(r => r.Name).ToList(),
    //            Permissions = user.Roles
    //                .SelectMany(r => r.Permissions)
    //                .Select(p => $"{p.Resource}:{p.Action}")
    //                .Distinct()
    //                .ToList()
    //        };

    //        return Ok(new ApiResponse<LoginResponse>
    //        {
    //            Success = true,
    //            Data = response
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error occurred while refreshing token");
    //        return StatusCode(500, new ApiResponse<LoginResponse>
    //        {
    //            Success = false,
    //            Message = "An error occurred while processing your request"
    //        });
    //    }
    //}
}