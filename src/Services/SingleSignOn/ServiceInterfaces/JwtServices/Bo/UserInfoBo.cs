namespace Eva.SingleSignOn.ServiceInterfaces.JwtServices.Bo;

public record UserInfoBo
{
    public string UserName { get; init; } = string.Empty;

    public string Avatar { get; init; } = string.Empty;

    public string[] Roles { get; init; } = Array.Empty<string>();
}