namespace Eva.SingleSignOn.Entities;

public class RoleBindUser : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }
}