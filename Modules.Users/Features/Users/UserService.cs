using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Common.Abstractions;
using Modules.Common.Domain.Results;
using Modules.Users.Database;
using Modules.Users.Domain.Authentication;
using Modules.Users.Domain.Errors;
using Modules.Users.Domain.Users;
using Modules.Users.Features.Users.Dtos;
using Modules.Users.Features.Users.Requests;

namespace Modules.Users.Features.Users;

public interface IUserService : IBaseService
{
    Task<Result<UserDto>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    Task<Result<LoginUserResponse>> LoginUserAsync(LoginUserRequest request, CancellationToken cancellationToken);
    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
    Task<Result<UserDto>> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<Result<UserDto>> UpdateUserAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken);
    Task<Result<Success>> DeleteUserAsync(string userId, CancellationToken cancellationToken);
    Task<Result<Success>> UpdateUserRoleAsync(string userId, UpdateUserRoleRequest request, CancellationToken cancellationToken);
}

internal sealed class UserService(
    UserManager<User> userManager,
    UsersDbContext context,
    IClientAuthorizationService authorizationService,
    ILogger<UserService> logger) : IUserService
{
    public async Task<Result<UserDto>> RegisterUserAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            UserName = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            logger.LogInformation("Failed to register user: {@Errors}", result.Errors);
            return UserErrors.RegistrationFailed(result.Errors);
        }

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            await userManager.AddToRoleAsync(user, request.Role);
        }

        logger.LogInformation("Created user with ID: {UserId}", user.Id);

        return new UserDto(user.Id, user.Email);
    }

    public async Task<Result<LoginUserResponse>> LoginUserAsync(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authorizationService.LoginAsync(request.Email, request.Password, cancellationToken);
        return result;
    }

    public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authorizationService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
        return result;
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
        {
            logger.LogInformation("User with ID {UserId} not found", userId);
            return UserErrors.NotFound(userId);
        }

        logger.LogInformation("Retrieved user with ID: {UserId}", userId);
        return new UserDto(user.Id, user.Email!);
    }

    public async Task<Result<UserDto>> UpdateUserAsync(
        string userId,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            logger.LogInformation("User with ID {UserId} not found", userId);
            return UserErrors.NotFound(userId);
        }

        user.Email = request.Email;
        user.UserName = request.Email;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            logger.LogError("Failed to update user: {@Errors}", result.Errors);
            return UserErrors.UpdateFailed(result.Errors);
        }

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            await userManager.AddToRoleAsync(user, request.Role);
        }

        logger.LogInformation("Updated user with ID: {UserId}", userId);

        return new UserDto(user.Id, user.Email);
    }

    public async Task<Result<Success>> DeleteUserAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            logger.LogInformation("User with ID {UserId} not found", userId);
            return UserErrors.NotFound(userId);
        }

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            logger.LogError("Failed to delete user: {@Errors}", result.Errors);
            return UserErrors.DeleteFailed(result.Errors);
        }

        logger.LogInformation("Deleted user with ID: {UserId}", userId);
        return Result.Success;
    }

    public async Task<Result<Success>> UpdateUserRoleAsync(
        string userId,
        UpdateUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authorizationService.UpdateUserRoleAsync(userId, request.NewRole, cancellationToken);
        return result;
    }
}
