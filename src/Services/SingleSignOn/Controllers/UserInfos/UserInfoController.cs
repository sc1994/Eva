using Eva.SingleSignOn.Controllers.UserInfos.Dto;
using Eva.SingleSignOn.Entities;

namespace Eva.SingleSignOn.Controllers.UserInfos;

public class UserInfoController : CrudController<UserInfo, UserInfoOutputDto, UserInfoCreateDto, UserInfoUpdateDto>
{
    public UserInfoController(IFreeSql freeSql, IMapper mapper) : base(freeSql, mapper)
    {
    }
}