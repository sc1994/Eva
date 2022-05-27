using Eva.SingleSignOn.Controllers.Users.Dto;

namespace Eva.SingleSignOn.Controllers.Users.Validator;

public class UserModifiedValidator : AbstractValidator<UserModifiedDto>
{
    public UserModifiedValidator()
    {
        RuleFor(x => x.UserName).VerifyUserName();
        RuleFor(x => x.Avatar).VerifyAvatar();
    }
}