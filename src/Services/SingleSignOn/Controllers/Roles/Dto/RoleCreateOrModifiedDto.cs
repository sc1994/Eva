using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.Roles.Dto;

[MapTo(typeof(Role))]
public record RoleCreateOrModifiedDto
{
    public string RoleName { get; set; } = string.Empty;
}