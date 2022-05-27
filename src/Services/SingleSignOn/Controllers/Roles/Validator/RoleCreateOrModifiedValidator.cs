using Eva.SingleSignOn.Controllers.Roles.Dto;

namespace Eva.SingleSignOn.Controllers.Roles.Validator;

public class RoleCreateOrModifiedValidator : AbstractValidator<RoleCreateOrModifiedDto>
{
    public RoleCreateOrModifiedValidator()
    {
        RuleFor(x => x.RoleName).VerifyRoleName();
    }
}