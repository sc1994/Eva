namespace Eva.SingleSignOn.ServiceInterfaces.UserServices;

public interface IUserInfoService
{
    Task<string> GetPasswordHashAsync(string password);
}