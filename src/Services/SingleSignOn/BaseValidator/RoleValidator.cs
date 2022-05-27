using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.BaseValidator;

public static class RoleValidator
{
    public static IRuleBuilderOptions<T, string> VerifyRoleName<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .NotEmpty()
            .WithMessage("用户名不能为空")
            .MaximumLength(Role.RoleNameMaxLength)
            .WithMessage("用户名长度不能超过{MaxLength}");
    }
}