namespace Eva.SingleSignOn.Entities;

public class UserInfo : BaseEntity
{
    public const int UserNameMaxLength = 20;
    public const int AvatarMaxLength = 300;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;
}