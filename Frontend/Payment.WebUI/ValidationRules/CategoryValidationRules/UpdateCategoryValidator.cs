using FluentValidation;
using Payment.WebUI.DTOs.CategoryDtos;

namespace Payment.WebUI.ValidationRules.CategoryValidationRules;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz")
            .GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori adı boş olamaz")
            .MaximumLength(50).WithMessage("Kategori adı en fazla 50 karakter olabilir")
            .MinimumLength(3).WithMessage("Kategori adı en az 3 karakter olabilir");
        RuleFor(x => x.Description).MaximumLength(500).WithMessage("Açıklama en fazla 250 karakter olabilir");
        RuleFor(x => x.ImagePath)
                .MaximumLength(200).WithMessage("Resim yolu en fazla 200 karakter olabilir");
    }
}
