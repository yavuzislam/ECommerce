using FluentValidation;
using Payment.DtoLayer.Dtos.ProductDtos;

namespace Payment.WebApi.ValidationRules.ProductValidationRules
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(I => I.Id).GreaterThan(0).WithMessage("Id 0'dan büyük olmalıdır")
                .NotEmpty().WithMessage("Id boş geçilemez");
            RuleFor(I => I.Name).NotEmpty().WithMessage("Ürün adı boş geçilemez");
            RuleFor(I => I.Description).NotEmpty().WithMessage("Ürün açıklaması boş geçilemez");
            RuleFor(I => I.Price).GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır");
            RuleFor(I => I.Stock).GreaterThan(0).WithMessage("Stok adedi 0'dan büyük olmalıdır");
            RuleFor(I => I.CategoryId).NotEmpty().WithMessage("Kategori Id boş geçilemez")
                .GreaterThan(0).WithMessage("Kategori Id 0'dan büyük olmalıdır");
            RuleFor(x => x.ImagePath)
                .MaximumLength(200).WithMessage("Resim yolu en fazla 200 karakter olabilir");
        }
    }
}
