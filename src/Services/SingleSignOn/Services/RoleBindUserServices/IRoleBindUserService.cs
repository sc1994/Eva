using Eva.SingleSignOn.Entities;
using Eva.SingleSignOn.ServiceInterfaces.RoleBindUserServices;
using FreeSql;

namespace Eva.SingleSignOn.Services.RoleBindUserServices;

public class RoleBindUserService : IRoleBindUserService
{
    private readonly IBaseRepository<RoleBindUser> _repo;
    private readonly IBaseRepository<Role> _repoRole;

    public RoleBindUserService(IFreeSql freeSql)
    {
        _repoRole = freeSql.GetRepository<Role>();
        _repo = freeSql.GetRepository<RoleBindUser>();
    }

    public async Task<bool> IsExistAsync(Guid roleId, Guid userId)
    {
        return await _repo.Select.Where(x => x.RoleId == roleId && x.UserId == userId).AnyAsync();
    }

    public async Task<string[]> GetRolesByUserIdAsync(Guid userId)
    {
        var bindRuleIds = await _repo.Select.Where(x => x.UserId == userId).ToListAsync(x => x.RoleId);
        
        var roleNames = await _repoRole.Select.Where(x => bindRuleIds.Contains(x.Id)).ToListAsync(x => x.RoleName);

        return roleNames.ToArray();
    }
}