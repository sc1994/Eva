using Eva.Demands.Controllers.DemandsItems.Dto;
using FluentValidation;

namespace Eva.Demands.Controllers.DemandsItems.Validator;

public class DemandsModifiedValidator : AbstractValidator<DemandsModifiedDto>
{
    public DemandsModifiedValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("需求名不能为空且不能超过100个字符");
        RuleFor(x => x.Description).MaximumLength(300).WithMessage("需求描述不能超过300个字符");
    }
}