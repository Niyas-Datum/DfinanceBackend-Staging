using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IRoleService
    {
        CommonResponse FillRoleMaster();
        CommonResponse FillRoleRights();
        CommonResponse FillRoleandRoleRights(int Id);
        CommonResponse AddRole(RoleDto roleDto);
        CommonResponse UpdateRole(RoleDto roleDto);
        CommonResponse DeleteRole(int Id);
    }
}
