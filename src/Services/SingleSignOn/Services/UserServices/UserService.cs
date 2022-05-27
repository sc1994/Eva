using System.Security.Cryptography;
using System.Text;
using Eva.SingleSignOn.Entities;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;
using FreeSql;

namespace Eva.SingleSignOn.Services.UserServices;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IBaseRepository<UserInfo> _repo;

    public UserService(IConfiguration configuration, IFreeSql freeSql)
    {
        _configuration = configuration;
        _repo = freeSql.GetRepository<UserInfo>();
    }

    public Task<string> GetPasswordHashAsync(string password)
    {
        var encoding = new ASCIIEncoding();
        var keyByte = encoding.GetBytes(_configuration["UserPasswordSecret"]);

        var messageBytes = encoding.GetBytes(password);
        using var hash256 = new HMACSHA256(keyByte);

        var hash = hash256.ComputeHash(messageBytes);
        return Task.FromResult(Convert.ToBase64String(hash));
    }

    public async Task<bool> IsExistPhoneAsync(string phone)
    {
        return await _repo.Select.Where(x => x.Phone == phone).AnyAsync();
    }

    public async Task<bool> IsExistAsync(Guid id)
    {
        return await _repo.Select.Where(x => x.Id == id).AnyAsync();
    }
}