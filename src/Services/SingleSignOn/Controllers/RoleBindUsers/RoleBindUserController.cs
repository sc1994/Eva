using Eva.SingleSignOn.Controllers.RoleBindUsers.Dto;
using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.RoleBindUsers;

public class RoleBindUserController : CrudController<RoleBindUser, RoleBindUserOutputDto, RoleBindUserCreateOrModifiedDto>
{
    private readonly IFreeSql _freeSql;

    public RoleBindUserController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
        _freeSql = freeSql;
    }

    public override async Task<RoleBindUserOutputDto> GetByIdAsync(Guid id)
    {
        var dto = await base.GetByIdAsync(id);

        using var uow = _freeSql.CreateUnitOfWork();
        dto.UserName = await uow.GetRepository<UserInfo>().Select.Where(x => x.Id == dto.UserId).FirstAsync(x => x.UserName);
        dto.RoleName = await uow.GetRepository<Role>().Select.Where(x => x.Id == dto.RoleId).FirstAsync(x => x.RoleName);

        return dto;
    }
}