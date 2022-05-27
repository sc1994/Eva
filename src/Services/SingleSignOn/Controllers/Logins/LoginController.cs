using System.ComponentModel.DataAnnotations;
using Eva.SingleSignOn.Controllers.Logins.Dto;
using Eva.SingleSignOn.Entities;
using Eva.SingleSignOn.ServiceInterfaces.JwtServices;
using Eva.SingleSignOn.ServiceInterfaces.JwtServices.Bo;
using Eva.SingleSignOn.ServiceInterfaces.RoleBindUserServices;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;
using Eva.ToolKit.Attributes;
using Eva.ToolKit.Exceptions;

namespace Eva.SingleSignOn.Controllers.Logins;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IFreeSql _freeSql;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IRoleBindUserService _roleBindUserService;

    public LoginController(IJwtService jwtService, IFreeSql freeSql, IUserService userService, IMapper mapper, IRoleBindUserService roleBindUserService)
    {
        _jwtService = jwtService;
        _freeSql = freeSql;
        _userService = userService;
        _mapper = mapper;
        _roleBindUserService = roleBindUserService;
    }

    [HttpPost]
    [FormatResponse]
    public async Task<TokenOutputDto> LoginAsync(LoginInputDto input)
    {
        var user = await _freeSql.GetRepository<UserInfo>()
            .Select
            .Where(x => x.Phone == input.Phone)
            .FirstAsync();

        if (user == null)
            throw new BusinessException("手机号码不存在", code: "400", subCode: "001");

        if (await _userService.GetPasswordHashAsync(input.Password) != user.Password)
            throw new BusinessException("密码错误", code: "400", subCode: "002");

        var bo = _mapper.Map<UserInfoBo>(user);
        bo.Roles = await _roleBindUserService.GetRolesByUserIdAsync(user.Id);
        var (token, expiration) = await _jwtService.GenerateJwtAsync(bo);

        return new TokenOutputDto(token, expiration);
    }

    [HttpGet]
    [FormatResponse]
    public async Task<UserOutputDto> GetUser([Required] [FromHeader(Name = "Authorization")] string token)
    {
        var user = await _jwtService.ParseJwt(token);

        return _mapper.Map<UserOutputDto>(user);
    }
}