using FluentValidation;

namespace Modules.Users.Features.Users.Requests;

public sealed record UpdateUserRoleRequest(string NewRole);

public class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
{
    public UpdateUserRoleRequestValidator()
    {
        RuleFor(x => x.NewRole)
            .NotEmpty().WithMessage("New role is required");
    }
}