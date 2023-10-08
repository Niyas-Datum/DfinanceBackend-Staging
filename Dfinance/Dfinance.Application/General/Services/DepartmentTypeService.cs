using Dfinance.Application.Dto;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Application.Services.General
{
    public class DepartmentTypeService: IDepartmentTypeService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public DepartmentTypeService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public CommonResponse FillDepartmentTypes()
        {
            var data = _context.SpReDepartmentTypeFillAllDepartment.FromSqlRaw("Exec DropDownListSP @Criteria = 'fillDepartmentTypes'").ToList();

            
            var maDepartments = data.Select(item => new SpReDepartmentTypeFillAllDepartment
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();

            return CommonResponse.Ok(maDepartments);
        }

        public CommonResponse AddDepartmentTypes(DepartmentTypeDto departmentTypeDto)
        {
            try
            {
                departmentTypeDto.CreatedBy = _authService.GetId().Value;
                ReDepartmentType departmenttype = new ReDepartmentType
                {
                    Department = departmentTypeDto.Department,
                    CreatedBy = departmentTypeDto.CreatedBy,
                    CreatedBranchId = 1,
                    CreatedOn = departmentTypeDto.CreatedOn,
                };
                string criteria = "InsertReDepartmentTypes";
                var result = _context.spDepartmentTypesC.FromSqlRaw($"EXEC spDepartmentTypes @Criteria='{criteria}',@Department='{departmenttype.Department}',@CreatedBy='{departmenttype.CreatedBy}',@BranchID='{1}',@CreatedOn='{departmenttype.CreatedOn}'").ToList();
                var NewId = result.Select(i => new spDepartmentTypesC
                {
                    NewID = i.NewID
                }).ToList();
                return CommonResponse.Created(NewId);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse UpdateDepartmentTypes(DepartmentTypeDto departmentTypeDto,int Id)
        {
            try
            {


                departmentTypeDto.CreatedBy = _authService.GetId().Value;
                if (Id == 0)
                    return CommonResponse.Error("Branch Not Found");

                else
                {
                    ReDepartmentType departmenttype = new ReDepartmentType
                    {
                        Department = departmentTypeDto.Department,
                        CreatedBy = departmentTypeDto.CreatedBy,
                        CreatedBranchId = 1,
                        CreatedOn = departmentTypeDto.CreatedOn,

                    };
                    string criteria = "UpdateReDepartmentTypes";
                    var result = _context.spDepartmentTypesC.FromSqlRaw($"EXEC spDepartmentTypes @Criteria='{criteria}',@ID='{Id}', @Department='{departmenttype.Department}',@CreatedBy='{departmenttype.CreatedBy}',@BranchID='{1}',@CreatedOn='{departmenttype.CreatedOn}'").ToList();
                return CommonResponse.Ok(result);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse DeleteDepartmentTypes(int Id)
        {
            try
            {
                int Mode = 3;
                string msg = "DepartmentTypes is Suspended";
                var result = _context.Database.ExecuteSqlRawAsync($"EXEC spDepartmentTypes @Mode='{Mode}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
    }
}
