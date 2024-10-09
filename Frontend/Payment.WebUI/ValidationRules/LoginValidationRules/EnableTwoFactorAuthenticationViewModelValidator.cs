using FluentValidation;
using Payment.WebUI.DTOs.LoginDtos;

namespace Payment.WebUI.ValidationRules.LoginValidationRules
{
    public class EnableTwoFactorAuthenticationViewModelValidator : AbstractValidator<EnableTwoFactorAuthenticationViewModel>
    {
        public EnableTwoFactorAuthenticationViewModelValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Kod alanı boş bırakılamaz")
                .Length(6).WithMessage("Kod 6 karakter olmalıdır");
        }
    }
}
