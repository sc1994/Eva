namespace Eva.SingleSignOn.ServiceInterfaces.RoleBindUserServices;

public interface IRoleBindUserService
{
    Task<bool> IsExistAsync(Guid roleId, Guid userId);
    
    Task<string[]> GetRolesByUserIdAsync(Guid userId);
}