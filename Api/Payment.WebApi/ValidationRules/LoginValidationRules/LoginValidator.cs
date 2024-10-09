using FluentValidation;
using Payment.DtoLayer.Dtos.LoginDtos;

namespace Payment.WebApi.ValidationRules.LoginValidationRules
{
    public class LoginValidator:AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş olamaz");
        }
    }
}
