using Eva.SingleSignOn.Controllers.RoleBindUsers.Dto;
using Eva.SingleSignOn.ServiceInterfaces.RoleBindUserServices;
using Eva.SingleSignOn.ServiceInterfaces.RoleServices;
using Eva.SingleSignOn.ServiceInterfaces.UserServices;

namespace Eva.SingleSignOn.Controllers.RoleBindUsers.Validator;

public class RoleBindUserCreateOrModifiedValidator : AbstractValidator<RoleBindUserCreateOrModifiedDto>
{
    private readonly IRoleBindUserService _roleBindUserService;

    public RoleBindUserCreateOrModifiedValidator(IRoleService roleService, IUserService userService, IRoleBindUserService roleBindUserService)
    {
        _roleBindUserService = roleBindUserService;
        RuleFor(x => x.RoleId).NotNull().WithMessage("规则Id不能为空")
            .Must(roleId => roleService.IsExistAsync(roleId).Result).WithMessage("规则Id不存在");
        RuleFor(x => x.UserId).NotNull().WithMessage("用户Id不能为空")
            .Must(userId => userService.IsExistAsync(userId).Result).WithMessage("角色Id不存在");

        RuleFor(x => x).Must(IsUnBind).WithMessage("角色和用户已存在绑定关系");
    }

    private bool IsUnBind(RoleBindUserCreateOrModifiedDto input)
    {
        return !_roleBindUserService.IsExistAsync(input.RoleId, input.UserId).Result;
    }
}