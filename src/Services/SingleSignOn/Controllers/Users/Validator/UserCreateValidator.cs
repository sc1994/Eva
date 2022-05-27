using Eva.SingleSignOn.Controllers.Users.Dto;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;

namespace Eva.SingleSignOn.Controllers.Users.Validator;

public class UserCreateValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateValidator(IUserService userService)
    {
        RuleFor(x => x.UserName).VerifyUserName();
        RuleFor(x => x.Password).VerifyPassword();
        RuleFor(x => x.Avatar).VerifyAvatar();
        RuleFor(x => x.RePassword).NotEmpty().WithMessage("确认密码不能为空");
        RuleFor(x => x.RePassword).Equal(x => x.Password).WithMessage("两次输入的密码不一致");
        RuleFor(x => x.Phone).VerifyPhoneAndIsExist(userService);
    }
}