using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.RoleBindUsers.Dto;

[MapTo(typeof(RoleBindUser))]
public record RoleBindUserCreateOrModifiedDto
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }
}