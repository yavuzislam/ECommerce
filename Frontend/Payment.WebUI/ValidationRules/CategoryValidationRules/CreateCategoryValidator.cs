using FluentValidation;
using Payment.WebUI.DTOs.CategoryDtos;

namespace Payment.WebUI.ValidationRules.CategoryValidationRules
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Kategori adı alanı boş geçilemez")
               .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir")
               .Matches("^[a-zA-ZğüşöçıİĞÜŞÖÇ ]+$").WithMessage("Kategori adı sadece harflerden oluşmalıdır");

            RuleFor(x => x.Description)
               .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");

            RuleFor(x => x.ImagePath)
                .MaximumLength(200).WithMessage("Resim yolu en fazla 200 karakter olabilir");

            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Resim dosyası boş olamaz");
        }
    }
}
