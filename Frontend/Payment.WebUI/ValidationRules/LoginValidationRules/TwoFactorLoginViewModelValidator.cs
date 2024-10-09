using FluentValidation;
using Payment.WebUI.DTOs.LoginDtos;

namespace Payment.WebUI.ValidationRules.LoginValidationRules
{
    public class TwoFactorLoginViewModelValidator : AbstractValidator<TwoFactorLoginViewModel>
    {
        public TwoFactorLoginViewModelValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Kod alanı boş bırakılamaz")
                .Length(6).WithMessage("Kod 6 karakter olmalıdır");
        }
    }
}
