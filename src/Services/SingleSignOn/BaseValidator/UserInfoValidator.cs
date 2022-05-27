using Eva.SingleSignOn.Entities;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;

namespace Eva.SingleSignOn.BaseValidator;

public static class UserInfoValidator
{
    public static IRuleBuilderOptions<T, string> VerifyUserName<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .NotEmpty()
            .WithMessage("用户名不能为空")
            .MaximumLength(UserInfo.UserNameMaxLength)
            .WithMessage("用户名长度不能超过{MaxLength}");
    }

    public static IRuleBuilderOptions<T, string> VerifyPassword<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .NotEmpty().WithMessage("密码不能为空")
            .Matches("(?=.*[0-9])(?=.*[a-zA-Z])(?=([\x21-\x7e]+)[^a-zA-Z0-9])").WithMessage("密码必须包含数字和字母以及至少一个特殊字符");
    }

    public static IRuleBuilderOptions<T, string> VerifyAvatar<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .NotEmpty().WithMessage("头像不能为空")
            .MaximumLength(UserInfo.AvatarMaxLength).WithMessage("头像长度不能超过{MaxLength}")
            .Matches(@"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%$#_]*)?").WithMessage("头像地址不正确");
    }

    public static IRuleBuilderOptions<T, string> VerifyPhone<T>(this IRuleBuilder<T, string> builder)
    {
        return builder
            .NotEmpty().WithMessage("手机号不能为空")
            .Matches(@"^1[34578]\d{9}$").WithMessage("手机号格式不正确");
    }

    public static IRuleBuilderOptions<T, string> VerifyPhoneAndIsExist<T>(this IRuleBuilder<T, string> builder, IUserService userService)
    {
        return builder
            .VerifyPhone()
            .Must(phone => !userService.IsExistPhoneAsync(phone).Result).WithMessage("手机号已存在");
    }
}