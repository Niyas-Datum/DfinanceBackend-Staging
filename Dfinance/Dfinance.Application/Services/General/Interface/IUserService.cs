using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IUserService
    {
        CommonResponse UserPopup();
        CommonResponse UserDropDown();
        CommonResponse FillUser();
        CommonResponse FillUserById(int Id);
        CommonResponse FillRole(int RoleId);
        CommonResponse GetRole();
        CommonResponse SaveUser(UserDto maEmployeeDetailsDto);
        CommonResponse UpdateUser(UserDto maEmployeeDetailsDto);
        CommonResponse Deleteuser(int Id);
        CommonResponse FillPettyCashAccount();
    }
}
