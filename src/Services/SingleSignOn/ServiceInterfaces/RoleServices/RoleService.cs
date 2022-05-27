namespace Eva.SingleSignOn.ServiceInterfaces.RoleServices;

public interface IRoleService
{
    Task<bool> IsExistAsync(Guid id);
}