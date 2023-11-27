﻿using Dfinance.Application.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.Interface.IGeneral
{
    public interface IDepartmentTypeService
    {
        CommonResponse DepartmentDropdown();
        CommonResponse FillDepartment();
        CommonResponse FillDepartmentById(int Id);
        //*********************departemntType*****************************************
        CommonResponse FillDepartmentTypes();
        CommonResponse FillDepartmentTypesById(int Id);
        CommonResponse SaveDepartmentTypes(DepartmentTypeDto departmentTypeDto);
        CommonResponse UpdateDepartmentTypes(DepartmentTypeDto departmentTypeDto,int Id);
        CommonResponse DeleteDepartmentTypes(int Id);

    }
}
