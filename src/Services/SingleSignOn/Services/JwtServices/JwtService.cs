using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Eva.SingleSignOn.Options;
using Eva.SingleSignOn.ServiceInterfaces.JwtServices;
using Eva.SingleSignOn.ServiceInterfaces.JwtServices.Bo;
using Microsoft.IdentityModel.Tokens;

namespace Eva.SingleSignOn.Services.JwtServices;

public class JwtService : IJwtService
{
    private readonly JwtSetting jwtSetting;

    public JwtService(JwtSetting jwtSetting)
    {
        this.jwtSetting = jwtSetting;
    }

    public async Task<(string token, int expiration)> GenerateJwtAsync(UserInfoBo data)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSetting.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(nameof(data.UserName), data.UserName),
                new Claim(nameof(data.Avatar), data.Avatar),
                new Claim(nameof(data.Roles), data.Roles.ConvertObjectToJson() ?? throw new NullReferenceException(nameof(data.Roles))),
            }),
            Expires = DateTime.UtcNow.AddSeconds(jwtSetting.Expiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(("Bearer " + tokenHandler.WriteToken(token), jwtSetting.Expiration));
    }

    public async Task<UserInfoBo> ParseJwt(string jwt)
    {
        if (!jwt.StartsWith("Bearer "))
        {
            throw new ArgumentException("token not start with Bearer ");
        }

        jwt = jwt.Substring("Bearer ".Length);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSetting.SecretKey);
        tokenHandler.ValidateToken(
            jwt,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            },
            out var validatedToken);

        var jwtToken = (JwtSecurityToken) validatedToken;
        if (jwtToken is null) throw new NullReferenceException(nameof(jwtToken));

        return await Task.FromResult(new UserInfoBo
        {
            UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserInfoBo.UserName))?.Value
                       ?? throw new NullReferenceException(nameof(UserInfoBo.UserName)),
            Avatar = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserInfoBo.Avatar))?.Value
                     ?? throw new NullReferenceException(nameof(UserInfoBo.Avatar)),
            Roles = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserInfoBo.Roles))?.Value.ConvertJsonToObject<string[]>()
                    ?? throw new NullReferenceException(nameof(UserInfoBo.Roles)),
        });
    }
}