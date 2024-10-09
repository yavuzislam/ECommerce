using FluentValidation;
using Payment.WebUI.DTOs.RegisterDtos;

namespace Payment.WebUI.ValidationRules.RegisterValidationRules
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Email is not valid");
        }
    }
}
