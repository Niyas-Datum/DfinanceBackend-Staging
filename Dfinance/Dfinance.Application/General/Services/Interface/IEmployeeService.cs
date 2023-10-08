using Dfinance.Application.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Interface.IGeneral
{
    public interface IEmployeeService
    {
        CommonResponse FillEmployees();
        //CommonResponse AddEmployee(MaEmployeeDetailsDto maEmployeeDetailsDto);
    }
}
