using Dfinance.Application.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Interface.IGeneral
{
    public interface IUserService
    {
        CommonResponse UserDropDown();
        CommonResponse FillUser();
        CommonResponse FillUserById(int Id);
        CommonResponse SaveUser(UserDto maEmployeeDetailsDto);
        CommonResponse UpdateUser(UserDto maEmployeeDetailsDto ,int Id);
        CommonResponse DeleteUserRight(int Id);
        CommonResponse DeleteBranchdetails(int Id);
        CommonResponse Deleteuser(int Id);
    }
}
