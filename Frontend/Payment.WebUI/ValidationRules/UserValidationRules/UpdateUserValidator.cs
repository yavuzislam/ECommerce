using FluentValidation;
using Payment.WebUI.DTOs.AppUserDtos;

namespace Payment.WebUI.ValidationRules.UserValidationRules;

public class UpdateUserValidator : AbstractValidator<UpdateAppUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty")
            .GreaterThan(0).WithMessage("Id must be greater than 0");
        RuleFor(x => x.UserName)
    .NotEmpty().WithMessage("Kullanıcı Adı Alanı Gereklidir")
    .Length(3, 20).WithMessage("Kullanıcı adı 3 ile 20 karakter arasında olmalıdır.")
    .Matches("^[abcçdefgğhıijklmnoöprsştuüvyzABCÇDEFGĞHİIJKLMNOÖPRSŞTUÜVYZ0123456789._@+-]+$")
    .WithMessage("Kullanıcı adı geçersiz karakterler içeriyor.");


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Mail Alanı Gereklidir")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad Alanı Gereklidir")
            .Length(3, 50).WithMessage("İsim 3 ile 50 karakter arasında olmalıdır.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Soyad Alanı Gereklidir")
        .Length(3, 50).WithMessage("Soyad 3 ile 50 karakter arasında olmalıdır.");

        RuleFor(x => x.PhoneNumber)
.NotEmpty().WithMessage("Telefon Alanı Gereklidir")
.Matches(@"^0\d{10}$").WithMessage("Geçerli bir telefon numarası giriniz.");
    }
}
