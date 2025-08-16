using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Common.API.Controllers;
using Modules.Users.Domain.Policies;
using Modules.Users.Features.Users.Requests;

namespace Modules.Users.Features.Users;

/// <summary>
/// Controller for user-related operations
/// </summary>
[Route("api/users")]
public class UsersController(IUserService userService) : BaseApiController
{
    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        [FromServices] IValidator<RegisterUserRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }

        var response = await userService.RegisterUserAsync(request, cancellationToken);
        return HandleCreateResult(response, nameof(GetUserById), new { userId = response.Value?.Id });
    }

    /// <summary>
    /// Login user
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginUserRequest request,
        [FromServices] IValidator<LoginUserRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }

        var response = await userService.LoginUserAsync(request, cancellationToken);
        return HandleResult(response);
    }

    /// <summary>
    /// Refresh authentication token
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        [FromServices] IValidator<RefreshTokenRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }

        var response = await userService.RefreshTokenAsync(request, cancellationToken);
        return HandleResult(response);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{userId}")]
    [Authorize(Policy = UserPolicyConsts.ReadPolicy)]
    public async Task<IActionResult> GetUserById(
        string userId,
        CancellationToken cancellationToken)
    {
        var response = await userService.GetUserByIdAsync(userId, cancellationToken);
        return HandleResult(response);
    }

    /// <summary>
    /// Update user
    /// </summary>
    [HttpPut("{userId}")]
    [Authorize(Policy = UserPolicyConsts.UpdatePolicy)]
    public async Task<IActionResult> UpdateUser(
        string userId,
        [FromBody] UpdateUserRequest request,
        [FromServices] IValidator<UpdateUserRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }

        var response = await userService.UpdateUserAsync(userId, request, cancellationToken);
        return HandleResult(response);
    }

    /// <summary>
    /// Delete user
    /// </summary>
    [HttpDelete("{userId}")]
    [Authorize(Policy = UserPolicyConsts.DeletePolicy)]
    public async Task<IActionResult> DeleteUser(
        string userId,
        CancellationToken cancellationToken)
    {
        var response = await userService.DeleteUserAsync(userId, cancellationToken);
        return HandleResult(response);
    }

    /// <summary>
    /// Update user role
    /// </summary>
    [HttpPut("{userId}/role")]
    [Authorize(Policy = UserPolicyConsts.UpdatePolicy)]
    public async Task<IActionResult> UpdateUserRole(
        string userId,
        [FromBody] UpdateUserRoleRequest request,
        [FromServices] IValidator<UpdateUserRoleRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }

        var response = await userService.UpdateUserRoleAsync(userId, request, cancellationToken);
        return HandleResult(response);
    }
}
