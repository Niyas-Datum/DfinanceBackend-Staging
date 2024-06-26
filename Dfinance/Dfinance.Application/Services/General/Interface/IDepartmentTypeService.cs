﻿using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IDepartmentTypeService
    {
        CommonResponse DepartmentDropdown();
        CommonResponse DeptPopup();
        CommonResponse FillDepartment();
        CommonResponse FillDepartmentById(int Id);
        CommonResponse AddDepartment(DepartmentTypeDto departmentDto);
        CommonResponse DeleteDepartmentTypes(int Id);

    }
}
