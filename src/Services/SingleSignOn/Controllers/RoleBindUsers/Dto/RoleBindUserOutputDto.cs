using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.RoleBindUsers.Dto;

[MapTo(typeof(RoleBindUser))]
public record RoleBindUserOutputDto : BasePrimaryKey
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    public string RoleName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}