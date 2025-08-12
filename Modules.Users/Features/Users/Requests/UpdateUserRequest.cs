using FluentValidation;

namespace Modules.Users.Features.Users.Requests;

public sealed record UpdateUserRequest(string Email, string? Role);

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");
    }
}