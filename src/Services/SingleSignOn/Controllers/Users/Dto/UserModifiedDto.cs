using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.Users.Dto;

[MapTo(typeof(UserInfo))]
public record UserModifiedDto
{
    public string UserName { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;
}