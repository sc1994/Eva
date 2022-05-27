using Eva.SingleSignOn.Controllers.Users.Dto;
using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.Users;

public class UserInfoController : CrudController<UserInfo, UserInfoOutputDto, UserCreateDto, UserModifiedDto>
{
    public UserInfoController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
    }
}