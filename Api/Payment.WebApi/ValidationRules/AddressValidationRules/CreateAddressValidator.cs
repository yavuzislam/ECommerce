using FluentValidation;
using Payment.DtoLayer.Dtos.AddressDtos;

namespace Payment.WebApi.ValidationRules.AddressValidationRules;

public class CreateAddressValidator : AbstractValidator<CreateAddressDto>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.AddressLine).NotEmpty().WithMessage("Adres alanı boş geçilemez.")
            .MaximumLength(255).WithMessage("Maksimum 255 karakter olabilir.");

        RuleFor(x => x.City).NotEmpty().WithMessage("Şehir alanı boş geçilemez.")
            .MaximumLength(100).WithMessage("Maksimum 100 karakter olabilir.");

        RuleFor(x => x.District).NotEmpty().WithMessage("İlçe alanı boş geçilemez.")
            .MaximumLength(100).WithMessage("Maksimum 100 karakter olabilir.");

        RuleFor(x => x.AppUserId).NotEmpty().WithMessage("Kullanıcı alanı boş geçilemez.")
            .GreaterThan(0).WithMessage("Kullanıcı alanı 0'dan büyük olmalıdır.");
    }
}
