using Eva.SingleSignOn.ServiceInterfaces.JwtServices.Bo;

namespace Eva.SingleSignOn.Controllers.Logins.Dto;

[MapTo(typeof(UserInfoBo))]
public record UserOutputDto
{
    public string UserName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;

    public string[] Roles { get; set; } = Array.Empty<string>();
}