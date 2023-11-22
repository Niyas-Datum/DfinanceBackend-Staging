using Dfinance.Application.Dto;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        //***********************DropDown***********************
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


        //**********************FillById********************************************************
        public CommonResponse FillDepartmentTypesById(int Id)
        {
            try
            {
                int mod = 9;
                var data = _context.spDepartmentTypesGetById
                    .FromSqlRaw($"Exec spDepartmentTypes @Mode='{mod}', @ID='{Id}'")
                    .ToList();
                
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching user by ID", ex);
            }
        }

        //************************AddDepartment****************************************************
        public CommonResponse AddDepartmentTypes(DepartmentTypeDto departmentTypeDto)
        {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;

                string criteria = "InsertReDepartmentTypes";

                SqlParameter newIdUserRight = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC spDepartmentTypes  @Criteria ={0},@Department={1},@CreatedBy={2},@BranchID={3},@CreatedOn={4},@NewID={5} OUTPUT",
                                                criteria, departmentTypeDto.Department, CreatedBy, CreatedBranchId, CreatedOn, newIdUserRight);

                int newIdValue = (int)newIdUserRight.Value;

                return CommonResponse.Created(newIdValue);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //*****************************UpdateDepartment****************************************************
        public CommonResponse UpdateDepartmentTypes(DepartmentTypeDto departmentTypeDto,int Id)
        {
            try
            {


                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                if (Id == 0)
                    return CommonResponse.Error("Branch Not Found");

                else
                {
                    
                    string criteria = "UpdateReDepartmentTypes";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spDepartmentTypes @Criteria='{criteria}',@ID='{Id}', @Department='{departmentTypeDto.Department}',@CreatedBy='{CreatedBy}',@BranchID='{CreatedBranchId}',@CreatedOn='{CreatedOn}'");
                return CommonResponse.Ok(result);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        //**************************Delete*****************************
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
