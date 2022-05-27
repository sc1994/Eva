namespace Eva.SingleSignOn.Entities;

public class Role : BaseEntity
{
    public const int RoleNameMaxLength = 20;
    
    public string RoleName { get; set; } = string.Empty;
}