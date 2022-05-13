using Eva.SingleSignOn.Const;
using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.UserInfos.Dto;

[MapTo(typeof(UserInfo))]
public record UserInfoCreateDto
{
    public string UserName { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    [Required] public string RePassword { get; set; } = string.Empty;

    [Required] public UniqueIdentity UniqueIdentityType { get; set; }

    [Required] public string UniqueIdentity { get; set; } = string.Empty;

    [Required] public string Avatar { get; set; } = string.Empty;
}