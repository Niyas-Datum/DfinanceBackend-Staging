using Dfinance.Application.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IDepartmentTypeService
    {
        CommonResponse DepartmentDropdown();
        CommonResponse FillDepartment();
        CommonResponse FillDepartmentById(int Id);
        //*********************departemntType*****************************************
        //CommonResponse FillDepartmentTypes();
        //CommonResponse FillDepartmentTypesById(int Id);
        CommonResponse AddDepartment(DepartmentTypeDto departmentDto);
        CommonResponse DeleteDepartmentTypes(int Id);

    }
}
