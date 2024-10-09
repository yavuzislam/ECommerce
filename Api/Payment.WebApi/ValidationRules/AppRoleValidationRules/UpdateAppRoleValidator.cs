//using FluentValidation;
//using Payment.DtoLayer.Dtos.AppRoleDto;

//namespace Payment.WebApi.ValidationRules.AppRoleValidationRules;

//public class UpdateAppRoleValidator : AbstractValidator<UpdateAppRoleDto>
//{
//    public UpdateAppRoleValidator()
//    {
//        RuleFor(p => p.Id).NotEmpty().WithMessage("Role id cannot be empty")
//            .GreaterThan(0).WithMessage("Role id must be greater than 0");
//        RuleFor(p => p.Name).NotEmpty().WithMessage("Role name cannot be empty");
//    }
//}
