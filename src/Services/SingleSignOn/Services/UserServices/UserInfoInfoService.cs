using System.Security.Cryptography;
using System.Text;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;

namespace Eva.SingleSignOn.Services.UserServices;

public class UserInfoInfoService : IUserInfoService
{
    private readonly IConfiguration _configuration;

    public UserInfoInfoService(IConfiguration configuration)
    {
        _configuration = configuration;
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
}