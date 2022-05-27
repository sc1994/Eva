using Eva.SingleSignOn.Entities;
using Eva.SingleSignOn.ServiceInterfaces.RoleServices;
using FreeSql;

namespace Eva.SingleSignOn.Services.RoleServices;

public class RoleService : IRoleService
{
    private readonly IBaseRepository<Role> _repo;

    public RoleService(IFreeSql freeSql)
    {
        _repo = freeSql.GetRepository<Role>();
    }

    public async Task<bool> IsExistAsync(Guid id)
    {
        return await _repo.Select.Where(x => x.Id == id).AnyAsync();
    }
}