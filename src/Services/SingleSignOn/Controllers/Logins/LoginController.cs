using Eva.SingleSignOn.Controllers.Logins.Dto;
using Eva.SingleSignOn.Entities;
using Eva.SingleSignOn.ServiceInterfaces.JwtServices;
using Eva.SingleSignOn.ServiceInterfaces.JwtServices.Bo;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;
using Eva.ToolKit.Attributes;
using Eva.ToolKit.Exceptions;

namespace Eva.SingleSignOn.Controllers.Logins;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly IFreeSql _freeSql;
    private readonly IUserInfoService _userInfoService;
    private readonly IMapper _mapper;

    public LoginController(IJwtService jwtService, IFreeSql freeSql, IUserInfoService userInfoService, IMapper mapper)
    {
        _jwtService = jwtService;
        _freeSql = freeSql;
        _userInfoService = userInfoService;
        _mapper = mapper;
    }

    [HttpPost]
    [FormatResponse]
    public async Task<TokenOutputDto> LoginAsync(LoginInputDto input)
    {
        var user = await _freeSql.GetRepository<UserInfo>()
            .Select
            .Where(x => !x.IsDeleted)
            .Where(x => x.UserName == input.UserName)
            .FirstAsync();

        if (user == null)
            throw new BusinessException("用户不存在", code: "400", subCode: "001");

        if (await _userInfoService.GetPasswordHashAsync(input.Password) != user.Password)
            throw new BusinessException("密码错误", code: "400", subCode: "002");

        var (token, expiration) = await _jwtService.GenerateJwtAsync(_mapper.Map<UserInfoBo>(user));

        return new TokenOutputDto(token, expiration);
    }

    [HttpGet]
    [FormatResponse]
    public async Task<UserOutputDto> GetUser([FromHeader(Name = "Authorization")] string token)
    {
        var user = await _jwtService.ParseJwt(token);

        return _mapper.Map<UserOutputDto>(user);
    }
}