using FluentValidation;
using Payment.WebUI.DTOs.AppRoleDto;

namespace Payment.WebUI.ValidationRules.AppRoleValidationRules;

public class CreateAppRoleValidator : AbstractValidator<CreateAppRoleDto>
{
    public CreateAppRoleValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Role name cannot be empty");
    }
}
