using Dfinance.Application.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Interface.IGeneral
{
    public interface IDepartmentTypeService
    {
        CommonResponse FillDepartmentTypes();
        CommonResponse FillDepartmentTypesById(int Id);
        CommonResponse AddDepartmentTypes(DepartmentTypeDto departmentTypeDto);
        CommonResponse UpdateDepartmentTypes(DepartmentTypeDto departmentTypeDto,int Id);
        CommonResponse DeleteDepartmentTypes(int Id);

    }
}
