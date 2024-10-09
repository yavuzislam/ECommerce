using FluentValidation;
using Payment.DtoLayer.Dtos.CategoryDtos;

namespace Payment.BusinessLayer.ValidationRules.CategoryValidationRules
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Kategori ID'si boş olamaz")
                .GreaterThan(0).WithMessage("Kategori ID'si 0'dan büyük olmalıdır");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı alanı boş geçilemez")
                .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir")
                .Matches("^[a-zA-ZğüşöçıİĞÜŞÖÇ ]+$").WithMessage("Kategori adı sadece harflerden oluşmalıdır");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");

            RuleFor(x => x.ImagePath)
                .MaximumLength(200).WithMessage("Resim yolu en fazla 200 karakter olabilir");
        }
    }
}
