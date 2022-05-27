using Eva.SingleSignOn.Controllers.Logins.Dto;

namespace Eva.SingleSignOn.Controllers.Logins.Validator;

public class LoginInputValidator : AbstractValidator<LoginInputDto>
{
    public LoginInputValidator()
    {
        RuleFor(x => x.Phone).VerifyPhone();
        RuleFor(x => x.Password).VerifyPassword();
    }
}