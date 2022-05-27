using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.ServiceInterfaces.JwtServices.Bo;

[MapTo(typeof(UserInfo))]
public record UserInfoBo
{
    public string UserName { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public string Avatar { get; init; } = string.Empty;

    public string[] Roles { get; set; } = Array.Empty<string>();
}