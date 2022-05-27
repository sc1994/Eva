using Eva.SingleSignOn.Entities;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;

namespace Eva.SingleSignOn.Controllers.Users.Dto;

public class UserProfile : Profile
{
    public UserProfile()
    {
        var userService = DIUtility.GetScopeRequiredService<IUserService>();
        
        CreateMap<UserCreateDto, UserInfo>()
            .ForMember(x => x.Password, opt => opt.MapFrom(x => userService.GetPasswordHashAsync(x.Password).Result));
    }
}