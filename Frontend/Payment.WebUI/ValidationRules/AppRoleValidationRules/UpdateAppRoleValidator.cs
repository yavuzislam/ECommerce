using FluentValidation;
using Payment.WebUI.DTOs.AppRoleDto;

namespace Payment.WebUI.ValidationRules.AppRoleValidationRules
{
    public class UpdateAppRoleValidator : AbstractValidator<UpdateAppRoleDto>
    {
        public UpdateAppRoleValidator()
        {
            RuleFor(p => p.ID).NotEmpty().WithMessage("Role id cannot be empty")
                .GreaterThan(0).WithMessage("Role id must be greater than 0");
            RuleFor(p => p.Name).NotEmpty().WithMessage("Role name cannot be empty");
        }
    }
}
