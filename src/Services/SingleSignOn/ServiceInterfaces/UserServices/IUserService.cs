namespace Eva.SingleSignOn.ServiceInterfaces.UserServices;

public interface IUserService
{
    Task<string> GetPasswordHashAsync(string password);

    Task<bool> IsExistPhoneAsync(string phone);

    Task<bool> IsExistAsync(Guid id);
}