using Eva.SingleSignOn.Controllers.Roles.Dto;
using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.Roles;

public class RoleController : CrudController<Role, RoleOutputDto, RoleCreateOrModifiedDto>
{
    public RoleController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
    }
}