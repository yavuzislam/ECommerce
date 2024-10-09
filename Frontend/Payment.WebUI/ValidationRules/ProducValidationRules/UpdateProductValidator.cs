using FluentValidation;
using Payment.WebUI.DTOs.ProductDtos;

namespace Payment.WebUI.ValidationRules.ProducValidationRules
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty")
                .GreaterThan(0).WithMessage("Id must be greater than 0");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty")
        .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty")
                .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price cannot be empty")
                .GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.DiscountRate).LessThanOrEqualTo(100).WithMessage("Discount rate must be less than or equal to 100");
            RuleFor(x => x.Stock).NotEmpty().WithMessage("Stock cannot be empty")
                .GreaterThan(0).WithMessage("Stock must be greater than 0");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category cannot be empty")
                .GreaterThan(0).WithMessage("Category must be greater than 0");
            RuleFor(x => x.ImagePath)
                .MaximumLength(200).WithMessage("Resim yolu en fazla 200 karakter olabilir");
        }
    }
}
