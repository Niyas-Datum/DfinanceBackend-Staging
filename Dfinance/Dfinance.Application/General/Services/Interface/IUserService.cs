using Dfinance.Application.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Interface.IGeneral
{
    public interface IUserService
    {
        CommonResponse FillUser();
        CommonResponse GetUserById(int Id);
        CommonResponse AddUser(UserDto maEmployeeDetailsDto);
        CommonResponse UpdateUser(UserDto maEmployeeDetailsDto ,int Id);
        CommonResponse DeleteUserRight(int Id);
        CommonResponse DeleteBranchdetails(int Id);
        CommonResponse Deleteuser(int Id);
    }
}
