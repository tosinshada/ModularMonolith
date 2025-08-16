using Microsoft.AspNetCore.Mvc;
using Modules.Common.Domain.Results;

namespace Modules.Common.API.Controllers;

/// <summary>
/// Base controller for API endpoints providing common functionality
/// </summary>
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Converts a result to an appropriate HTTP response
    /// </summary>
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsError)
        {
            return ConvertToProblem(result.Errors);
        }

        return result.Value switch
        {
            null => NoContent(),
            _ => Ok(result.Value)
        };
    }

    /// <summary>
    /// Converts a result to an appropriate HTTP response for creation scenarios
    /// </summary>
    protected IActionResult HandleCreateResult<T>(Result<T> result, string actionName, object routeValues)
    {
        return result.IsError 
            ? ConvertToProblem(result.Errors) 
            : CreatedAtAction(actionName, routeValues, result.Value);
    }

    /// <summary>
    /// Converts errors to appropriate problem response
    /// </summary>
    private IActionResult ConvertToProblem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        var statusCode = errors.First().Type switch
        {
            ErrorType.Conflict => 409,
            ErrorType.Validation => 400,
            ErrorType.NotFound => 404,
            ErrorType.Unauthorized => 401,
            ErrorType.Forbidden => 403,
            ErrorType.Failure => 400,
            ErrorType.Unexpected => 400,
            ErrorType.Custom => 400,
            _ => 500
        };

        var problemDetails = errors.ToDictionary(k => k.Code, v => new[] { v.Description });
        return ValidationProblem(new ValidationProblemDetails(problemDetails) { Status = statusCode });
    }
}
