using Eva.SingleSignOn.ServiceInterfaces.JwtServices.Bo;

namespace Eva.SingleSignOn.ServiceInterfaces.JwtServices;

public interface IJwtService
{
    Task<(string token, int expiration)> GenerateJwtAsync(UserInfoBo data);

    Task<UserInfoBo> ParseJwt(string jwt);
}